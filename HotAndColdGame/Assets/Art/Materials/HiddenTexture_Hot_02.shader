// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Hidden Texture" {
    Properties{
        _DiffuseTex("Diffuse", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _SpotAngle("Spot Angle", Float) = 30.0
        _Range("Range", Float) = 5.0
        _Contrast("Contrast", Range(20.0, 80.0)) = 50.0
        _Alpha("Alpha",  Range(0.0, 1.0)) = 1.0
        _Color_Value("Colour_Value",  Range(0.0, 1.0)) = 0.5
        _Bump_Value("Bump_Value",  Range(-1.0, 1.0)) = 0

        _SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess("Shininess", Range(0.01, 1)) = 0.078125
        _Gloss("Gloss", Range(0.0, 2.0)) = 1
        _GlossMap("Gloss Map", 2D) = "gloss" {}
    }

        Subshader{
            Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}
            Pass {
                Blend SrcAlpha OneMinusSrcAlpha
                ZTest Always

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _DiffuseTex;
                float4 _DiffuseTex_ST;
                uniform sampler2D _BumpMap; // normal map
                float4 _BumpMap_ST;

                uniform float4 _LightPos; // light world position - set via script
                uniform float4 _LightDir; // light world direction - set via script
                uniform float _SpotAngle; // spotlight angle
                uniform float _Range; // spotlight range
                uniform float _Contrast; // adjusts contrast
                uniform float _Alpha; // adjusts transparency
                uniform float _Color_Value; // adjusts color value
                uniform float _Bump_Value; // adjusts bump value

                half _Shininess;
                half _Gloss;
                sampler2D _GlossMap;
                float4 _GlossMap_ST;

                struct v2f_interpolated {
                    float4 pos : SV_POSITION;
                    float2 texCoord : TEXCOORD0;
                    float3 lightDir : TEXCOORD1;

                    half3 tspace0 : TEXCOORD2; // tangent.x, bitangent.x, normal.x
                    half3 tspace1 : TEXCOORD3; // tangent.y, bitangent.y, normal.y
                    half3 tspace2 : TEXCOORD4; // tangent.z, bitangent.z, normal.z

                    half3 viewDir : TEXCOORD5;
                    half3 normalDir : TEXCOORD6;
                };

                v2f_interpolated vert(appdata_full v) {
                    v2f_interpolated o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.texCoord = v.texcoord;

                    o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                    o.normalDir = v.normal;

                    half3 wNormal = UnityObjectToWorldNormal(v.normal);
                    half3 wTangent = UnityObjectToWorldDir(v.tangent.xyz);
                    // compute bitangent from cross product of normal and tangent
                    half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
                    half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
                    // output the tangent space matrix
                    o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                    o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                    o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);

                    half3 worldSpaceVertex = mul(unity_ObjectToWorld, v.vertex).xyz;
                    // calculate light direction to vertex    
                    o.lightDir = worldSpaceVertex - _LightPos.xyz;
                    return o;
                    //    v2f_interpolated o;
                    //    o.texCoord.xy = v.texcoord.xy;
                    //    o.pos = UnityObjectToClipPos(v.vertex);
                    //    half3 worldSpaceVertex = mul(unity_ObjectToWorld, v.vertex).xyz;
                    //    // calculate light direction to vertex    
                    //    o.lightDir = worldSpaceVertex - _LightPos.xyz;
                    //    return o;
                    //}
                }

                half4 frag(v2f_interpolated i) : SV_Target {

                    half2 uv_MainTex = TRANSFORM_TEX(i.texCoord, _DiffuseTex);
                    half2 uv_BumpMap = TRANSFORM_TEX(i.texCoord, _BumpMap);

                    half3 tnormal = UnpackNormal(tex2D(_BumpMap, uv_BumpMap));
                    // transform normal from tangent to world space
                    half3 worldNormal;
                    worldNormal.x = dot(i.tspace0, tnormal);
                    worldNormal.y = dot(i.tspace1, tnormal);
                    worldNormal.z = dot(i.tspace2, tnormal);

                    //half diffuse = saturate(dot(_LightDir.xyz, worldNormal) * 1.2);
                    half diffuse = saturate(worldNormal * _Bump_Value);
                    
                    half3 colorSample = tex2D(_DiffuseTex, uv_MainTex).rgb;

                    half dist = saturate(1 - (length(i.lightDir) / _Range)); // get distance factor
                    half cosLightDir = dot(normalize(i.lightDir), normalize(_LightDir)); // get light angle
                    half ang = cosLightDir - cos(radians(_SpotAngle / 2)); // calculate angle factor
                    half alpha = saturate(dist * ang * _Contrast); // combine distance, angle and contrast

                    half4 result;
                    result.a = alpha * _Alpha;
                    result.rgb = (colorSample) + diffuse;
                    
                    return result;

                    //half dist = saturate(1 - (length(i.lightDir) / _Range)); // get distance factor
                    //half cosLightDir = dot(normalize(i.lightDir), normalize(_LightDir)); // get light angle
                    //half ang = cosLightDir - cos(radians(_SpotAngle / 2)); // calculate angle factor
                    //half alpha = saturate(dist * ang * _Contrast); // combine distance, angle and contrast
                    //half4 c = tex2D(_DiffuseTex, i.texCoord); // get texel
                    //c.a *= alpha * _Alpha; // combine texel and calculated alpha
                    //return c;

                    //half4 result;
                    //result.rgb = (colorSample)*diffuse;

                    ////result.a *= alpha * _Alpha; // combine texel and calculated alpha
                    //return result;
                }
                ENDCG
            }
        }
}