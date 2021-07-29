// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'glstate.matrix.texture[0]' with 'UNITY_MATRIX_TEXTURE0'

Shader "Custom/Hidden Texture" {
    Properties{

        // surface properties
         _Color("Surface Color", Color) = (1,1,1,1)
         _SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)

         _SurfaceTex("Surface Texture", 2D) = "white" {}
         _BumpMap("Normal Map", 2D) = "bump" {}
         //_SpecMap("Specular map", 2D) = "black" {}

         _Alpha("Alpha",  Range(0.0, 1.0)) = 1.0
         _Shininess("Shininess", Range(0.00, 1)) = 0.05
         _BumpValue("Bump Value",  Range(0.0, 1.0)) = 0.5
         _Contrast("Contrast", Range(1.0, 80.0)) = 2.0
         //_Height("Height", Range(-1.0, 1.0)) = 0.0
         // light properties
         _Range("Range", Float) = 5.0
         // rim properties
         _RimPower("Rim Power", Range(-2, 2)) = 0.01
         _RimColour("Rim Colour", Color) = (1,1,1,1)

        // frensel properties
        _FresnelColor("Fresnel Color", Color) = (1,1,1,1)
        _FresnelBias("Fresnel Bias", Float) = 0
        _FresnelScale("Fresnel Scale", Float) = 1
        _FresnelPower("Fresnel Power", Float) = 1

    }

        Subshader{
            Tags {"RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent"}

            Pass {
                Blend SrcAlpha One
                ZTest On

                Offset -1, -1

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fwdbase
                #include "UnityCG.cginc"
                float4 _Color;
                float4 _SpecColor;
                uniform float _Shininess;
                sampler2D _SurfaceTex;  // surface texture 
                float4 _SurfaceTex_ST;

                //sampler2D _SpecMap;  // specular map
                //float4 _SpecMap_ST;

                uniform sampler2D _BumpMap; // normal map
                float4 _BumpMap_ST;

                uniform float4 _LightPos; // light world position - set via script
                uniform float4 _LightDir; // light world direction - set via script
                uniform float _Range; // spotlight range
                uniform float _Contrast; // adjusts contrast
                uniform float _Alpha; // adjusts transparency
                uniform float _BumpValue; // adjusts bump amount
                uniform float _RimPower; // adjusts rim value
                float4 _RimColour;
                //uniform float _Height; // adjusts height

                fixed4 _FresnelColor;
                fixed _FresnelBias;
                fixed _FresnelScale;
                fixed _FresnelPower;


                struct v2f_interpolated {
                    float4 pos : SV_POSITION;
                    float2 texCoord : TEXCOORD0;
                    float3 lightDir : TEXCOORD1;

                    half3 tspace0 : TEXCOORD2; // tangent.x, bitangent.x, normal.x
                    half3 tspace1 : TEXCOORD3; // tangent.y, bitangent.y, normal.y
                    half3 tspace2 : TEXCOORD4; // tangent.z, bitangent.z, normal.z

                    half3 viewDir : TEXCOORD5;
                    half3 normalDir : TEXCOORD6;
                    float fresnel : TEXCOORD7;

                    float4 uvrefr : TEXCOORD8;
                    float2 uv : TEXCOORD9;
                };



                v2f_interpolated vert(appdata_full v) {

                    /*float3 pos = v.vertex.xyz;
                    pos.y += (_Height * 0.01);
                    v.vertex.xyz = pos;*/

                    v2f_interpolated o;
                    o.pos = UnityObjectToClipPos(v.vertex);

                    o.texCoord = v.texcoord;

                    // refraction
                    o.uv = TRANSFORM_UV(1);
                    o.uvrefr = mul(UNITY_MATRIX_TEXTURE0, v.vertex);

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

                    // calculate frensel
                    //float3 i = normalize(ObjSpaceViewDir(v.pos));
                    //o.fresnel = _FresnelBias + _FresnelScale * pow(1 + dot(o.viewDir, v.normal), _FresnelPower);

                    
                   


                    return o;
                }

                half4 frag(v2f_interpolated i) : SV_Target {

                    half2 uv_MainTex = TRANSFORM_TEX(i.texCoord, _SurfaceTex);
                    half2 uv_BumpMap = TRANSFORM_TEX(i.texCoord, _BumpMap);

                    half3 tnormal = UnpackNormal(tex2D(_BumpMap, uv_BumpMap));
                    // transform normal from tangent to world space
                    half3 worldNormal;
                    worldNormal.x = dot(i.tspace0, tnormal);
                    worldNormal.y = dot(i.tspace1, tnormal);
                    worldNormal.z = dot(i.tspace2, tnormal);

                    half diffuse = saturate(dot(_LightDir.xyz, worldNormal) * 1.2 * _BumpValue);
                    //half diffuse = worldNormal * 1.2;

                    half3 colorSample = tex2D(_SurfaceTex, uv_MainTex).rgb * _Color;

                    half dist = saturate(1 - (length(i.lightDir) / _Range)); // get distance factor
                    //half cosLightDir = dot(normalize(i.lightDir), normalize(_LightDir)); // get light angle
                    //half ang = cosLightDir - cos(radians(_SpotAngle / 2)); // calculate angle factor          < for spotlights
                    //half alpha = saturate(dist * ang * _Contrast); // combine distance, angle and contrast    < for spotlights
                    half alpha = saturate(dist * _Contrast); // combine distance and contrast                   < for point lights

                    // specular
                    float3 vertex_normal = normalize(worldNormal);
                    float facing = max(0, dot(_LightDir, vertex_normal));
                    float3 view_dir = normalize(_WorldSpaceCameraPos - i.lightDir);
                    float3 half_way = normalize(view_dir + _LightDir);
                    float specular = facing * pow(max(dot(vertex_normal, half_way), 0),1);
                    //float specular = cosLightDir * _Shininess;

                    half4 result;
                    result.a = alpha * _Alpha * 10;
                    // add colour
                    result.rgb = colorSample * _Color; 
                    // add fresnel
                    //result.rgb += lerp(result.rgb, _FresnelColor, 1 - i.fresnel);
                    // add shinyness
                    result.rgb += (specular * _Shininess * _SpecColor);
                    



                    //result = lerp(result, _FresnelColor, 1 - i.fresnel);
                    //// add rim
                    //half rim = 1.0 - saturate(dot(normalize(view_dir), vertex_normal));
                    //result.rgb = lerp(result.rgb, result.rgb * _RimColour.rgb, pow(rim, _RimPower));

                    return result;
                }
                ENDCG
            }
         }
}