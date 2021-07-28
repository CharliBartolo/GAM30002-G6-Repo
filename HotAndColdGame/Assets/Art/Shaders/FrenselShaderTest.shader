Shader "Custom/FrenselShaderTest"
{
	Properties
	{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_FresnelColor("Fresnel Color", Color) = (1,1,1,1)
		_FresnelBias("Fresnel Bias", Float) = 0
		_FresnelScale("Fresnel Scale", Float) = 1
		_FresnelPower("Fresnel Power", Float) = 1

		_Alpha("Alpha",  Range(0.0, 1.0)) = 1.0
		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess("Shininess", Range(0.00, 1)) = 0.05
	}

		SubShader
		{
			Tags {"RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent"}

			Cull Back


			Pass
			{
				Blend SrcAlpha One
				ZTest On

				Offset -1, -1

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0

				#include "UnityCG.cginc"
				
				
				
				struct appdata_t
				{
					float4 pos : POSITION;
					float2 uv : TEXCOORD0;
					half3 normal : NORMAL;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
					half2 uv : TEXCOORD0;
					float fresnel : TEXCOORD1;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _Color;

				fixed4 _FresnelColor;
				fixed _FresnelBias;
				fixed _FresnelScale;
				fixed _FresnelPower;

				uniform float _Alpha; // adjusts transparency
				float4 _SpecColor; // adjusts specular colour
				uniform float _Shininess; // adjusts shininess value

				v2f vert(appdata_t v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.pos);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);

					float3 i = normalize(ObjSpaceViewDir(v.pos));
					o.fresnel = _FresnelBias + _FresnelScale * pow(1 + dot(i, v.normal), _FresnelPower);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 c = tex2D(_MainTex, i.uv) * _Color;
					c = lerp(c, _FresnelColor, 1 - i.fresnel);
					c.a = _Alpha * 10;
					return c;
				}
				ENDCG
			}
		}
}