Shader "Custom/Hidden_CrystalIce"
{
	Properties
	{
		_Alpha("Alpha",  Range(0.0, 1.0)) = 1.0
		_Contrast("Contrast", Range(1.0, 80.0)) = 2.0

		_LayerTex("Layer Texture", 2D) = "white" {}
		_LayerTint("Layer Tint", COLOR) = (1,1,1,1)
		_LayerHeightBias("Layer Height Start Bias", Range(0.0, 0.2)) = 0.1

		_HeightTex("Heightmap Texture", 2D) = "black" {}
		_HeightScale("Height Scale", Range(0.0, 0.5)) = 0.1

		_FresnelTightness("Fresnel Tightness", Range(0.0, 10.0)) = 4.0
		_FresnelColorInside("Fresnel Color Inside", COLOR) = (1,1,0.5,1)
		_FresnelColorOutside("Fresnel Color Outside", COLOR) = (1,1,1,1)

		_SurfaceAlphaMaskTex("Surface Alpha Mask Texture", 2D) = "white" {}
		_SurfaceAlphaColor("Surface Mask Color", COLOR) = (1,1,1,1)

		_InnerLightTightness("Inner Light Tightness", Range(0.0, 40.0)) = 40.0
		_InnerLightColorInside("Inner Light Color Inside", COLOR) = (0,0,0,0)
		_InnerLightColorOutside("Inner Light Color Outside", COLOR) = (0,0,0,0)

		_SpecularTightness("Specular Tightness", Range(0.0, 40.0)) = 2.0
		_SpecularBrightness("Specular Brightness", Range(0.0, 1.0)) = 1.0

		_Cubemap("CubeMap", CUBE) = ""{}
		_RefractionStrength("Refraction Strength", Range(0.0, 1.0)) = 0.2

	}
		SubShader
		{
			Tags { "RenderType" = "Transparent"  "IgnoreProjector" = "True" "Queue" = "Transparent"}
			LOD 100

			Pass
			{
				Blend SrcAlpha One
				ZTest On
				Offset -1, -1

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				#include "Lighting.cginc"
				#include "AutoLight.cginc"
				struct appdata
				{
					float4 pos : POSITION;
					float3 normal : NORMAL;
					float3 tangent : TANGENT;
					float2 uv : TEXCOORD0;
				};
				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 pos : SV_POSITION;

					float4 lightData : TEXCOORD1;
					float3 worldPos: TEXCOORD2;
					float3 worldNormal: TEXCOORD3;
					float3 worldViewDir: TEXCOORD5;
					float3 camPosTexcoord : TEXCOORD6;
					float4 screenPos:TEXCOORD7;
					float3 viewNormal : TEXCOORD8;
					float3 lightDir : TEXCOORD9;
				};

					sampler2D _LayerTex;
					fixed4 _LayerTint,_LayerTex_ST;
					float _LayerHeightBias;

					// marble tex
					sampler2D _HeightTex;
					float4 _HeightTex_ST;
					float _HeightScale;

					// surface alpha masking
					sampler2D _SurfaceAlphaMaskTex;
					float4 _SurfaceAlphaMaskTex_ST;
					float4 _SurfaceAlphaColor;

					// fresnel
					float _FresnelTightness;
					float4 _FresnelColorInside;
					float4 _FresnelColorOutside;

					// inner light
					float _InnerLightTightness;
					float4 _InnerLightColorOutside,_InnerLightColorInside;

					// specular
					float _SpecularTightness,_SpecularBrightness;

					// refraction
					float _RefractionStrength;
					samplerCUBE _Cubemap;

					uniform float4 _LightPos; // light world position - set via script
					uniform float4 _LightDir; // light world direction - set via script
					uniform float _Range; // spotlight range
					uniform float _Contrast; // adjusts contrast
					uniform float _Alpha; // adjusts transparency


				float lumin(float3 rgb)
				{
					return dot(rgb, float3(0.299, 0.587, 0.114));
				}
				v2f vert(appdata v)
				{
					float3 localPos = v.pos;
					float3 worldPos = mul(unity_ObjectToWorld, v.pos).xyz;
					float3 worldNormal = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyz);
					float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
					float3 binormal = cross(v.tangent, v.normal);
					float3x3 tbn = float3x3(v.tangent, binormal, v.normal);
					float3 camPosLocal = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1.0)).xyz;
					float3 dirToCamLocal = camPosLocal - localPos;
					float3 camPosTexcoord = mul(tbn, dirToCamLocal);
					half3 worldSpaceVertex = mul(unity_ObjectToWorld, v.pos).xyz;

					v2f o;
					o.pos = UnityObjectToClipPos(localPos);
					o.uv = v.uv;
					o.worldNormal = worldNormal;
					o.worldPos = worldPos;
					o.worldViewDir = worldViewDir;
					o.camPosTexcoord = camPosTexcoord;
					o.screenPos = ComputeScreenPos(o.pos);
					o.viewNormal = normalize(mul(UNITY_MATRIX_MV, float4(v.normal, 0.0)).xyz);
					o.lightDir = worldSpaceVertex - _LightPos.xyz;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{

					float phong = saturate(dot(i.worldNormal, normalize(_WorldSpaceCameraPos - i.worldPos)));
					float3 eyeVec = normalize(i.camPosTexcoord);
					float height = tex2D(_HeightTex, TRANSFORM_TEX(i.uv, _HeightTex)).r;
					float v = height * _HeightScale - (_HeightScale * 0.5);
					float2 newCoords = i.uv + eyeVec.xy * v;

					// accumulate layers
					float3 colorLayersAccum = float3(0.0, 0.0, 0.0);
					float layerDepthFalloffAccum = 1.0;
					float layerHeightBiasAccum = _LayerHeightBias;


					float2 layerBaseUV = TRANSFORM_TEX(i.uv, _LayerTex);
					float2 layerParallaxUV = layerBaseUV + eyeVec.xy * v + eyeVec.xy * -layerHeightBiasAccum;

					colorLayersAccum += tex2D(_LayerTex, layerParallaxUV).xyz * layerDepthFalloffAccum * _LayerTint.xyz;

					float3 color = colorLayersAccum;
					float alpha = 0.0;

					// light direction stuff
					half dist = saturate(1 - (length(i.lightDir) / _Range)); // get distance factor
					
					float3 view_dir = normalize(_WorldSpaceCameraPos - i.lightDir);
					float3 half_way = normalize(view_dir + _LightDir);

					// marble
					fixed4 texMarble = tex2D(_HeightTex, TRANSFORM_TEX(newCoords, _HeightTex));
					color += texMarble.xyz;


					// alpha everything so far
					alpha += saturate(lumin(color));

					// fresnel
					float fresnel = pow(1.0 - phong, _FresnelTightness);
					color += lerp(_FresnelColorInside, _FresnelColorOutside, fresnel) * fresnel;
					alpha += fresnel;

					// inner light
					float innerLight = pow(phong, _InnerLightTightness);
					color += lerp(_InnerLightColorOutside, _InnerLightColorInside, innerLight) * innerLight;
					alpha += innerLight;

					// overall alpha mask
					float alphaMask = tex2D(_SurfaceAlphaMaskTex, TRANSFORM_TEX(i.uv, _SurfaceAlphaMaskTex)).r;
					color = color + _SurfaceAlphaColor.xyz * alphaMask;
					alpha += alphaMask;

					// specular
					float3 worldNormalNormalized = normalize(i.worldNormal);
					float3 R = reflect(-_WorldSpaceLightPos0.xyz, worldNormalNormalized);
					float specular = pow(saturate(dot(R, normalize(i.worldViewDir))), _SpecularTightness);
					color += _LightColor0.xyz * specular * _SpecularBrightness;
					alpha += specular * _SpecularBrightness;

					color = saturate(color);
					alpha = alpha * _Alpha;
					alpha = saturate(alpha);
					
					// area mask
					half alpha_m = saturate(dist * _Contrast);
					

					// refraction
					float3 screenUV = float3(i.screenPos.xy / i.screenPos.w,1);
					half4 bgcolor = texCUBE(_Cubemap,screenUV + (-i.viewNormal.xyz * 0.5 + float3(height, 1,0)) * _RefractionStrength);
					color = lerp(bgcolor.xyz, color, alpha);
					alpha = 1.0;

					half4 result;
					result.a = alpha_m ;
					result.rgb = color;
					

					//return float4(color, alpha);
					return result;
				}
				ENDCG
			}
		}
}