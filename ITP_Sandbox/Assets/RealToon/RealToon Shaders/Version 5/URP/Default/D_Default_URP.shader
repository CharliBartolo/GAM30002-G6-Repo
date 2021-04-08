//RealToon V5.0.7 (URP)
//MJQStudioWorks
//2020

Shader "Universal Render Pipeline/RealToon/Version 5/Default/Default"
{
    Properties
    {

		[Enum(Off,2,On,0)] _DoubleSided("Double Sided", int) = 2

        _MainTex ("Texture", 2D) = "white" {}
        [Toggle(NOKEWO)] _TexturePatternStyle ("Texture Pattern Style", Float ) = 0
        [HDR] _MainColor ("Main Color", Color) = (0.2156863,0.2156863,0.2156863,1)

		[Toggle(NOKEWO)] _MVCOL ("Mix Vertex Color", Float ) = 0

		[Toggle(NOKEWO)] _MCIALO ("Main Color In Ambient Light Only", Float ) = 0

		[HDR] _HighlightColor ("Highlight Color", Color) = (1,1,1,1)
        _HighlightColorPower ("Highlight Color Power", Float ) = 1

        [Toggle(NOKEWO)] _EnableTextureTransparent ("Enable Texture Transparent", Float ) = 0

		_MCapIntensity ("Intensity", Range(0, 1)) = 1
		_MCap ("MatCap", 2D) = "white" {}
		[Toggle(NOKEWO)] _SPECMODE ("Specular Mode", Float ) = 0
		_SPECIN ("Specular Power", Float ) = 1
		_MCapMask ("Mask MatCap", 2D) = "white" {}

        _Cutout ("Cutout", Range(0, 1)) = 0
		[Toggle(NOKEWO)] _AlphaBaseCutout ("Alpha Base Cutout", Float ) = 1
        [Toggle(NOKEWO)] _UseSecondaryCutout ("Use Secondary Cutout", Float ) = 0
        _SecondaryCutout ("Secondary Cutout", 2D) = "white" {}

        _NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalMapIntensity ("Normal Map Intensity", Float ) = 1

        _Saturation ("Saturation", Range(0, 2)) = 1

        _OutlineWidth ("Width", Float ) = 0.5
        _OutlineWidthControl ("Width Control", 2D) = "white" {}

		[Enum(Normal,0,Origin,1)] _OutlineExtrudeMethod("Outline Extrude Method", int) = 0

		_OutlineOffset ("Outline Offset", Vector) = (0,0,0)
		_OutlineZPostionInCamera ("Outline Z Position In Camera", Float) = 0

		[Enum(Off,1,On,0)] _DoubleSidedOutline("Double Sided Outline", int) = 1

        [HDR] _OutlineColor ("Color", Color) = (0,0,0,1)

		[Toggle(NOKEWO)] _MixMainTexToOutline ("Mix Main Texture To Outline", Float ) = 0

        _NoisyOutlineIntensity ("Noisy Outline Intensity", Range(0, 1)) = 0
        [Toggle(NOKEWO)] _DynamicNoisyOutline ("Dynamic Noisy Outline", Float ) = 0
        [Toggle(NOKEWO)] _LightAffectOutlineColor ("Light Affect Outline Color", Float ) = 0

        [Toggle(NOKEWO)] _OutlineWidthAffectedByViewDistance ("Outline Width Affected By View Distance", Float ) = 0
		_FarDistanceMaxWidth ("Far Distance Max Width", Float ) = 10

        [Toggle(NOKEWO)] _VertexColorBlueAffectOutlineWitdh ("Vertex Color Blue Affect Outline Witdh", Float ) = 0

        _SelfLitIntensity ("Intensity", Range(0, 1)) = 0
        [HDR] _SelfLitColor ("Color", Color) = (1,1,1,1)
        _SelfLitPower ("Power", Float ) = 2
		_TEXMCOLINT ("Texture and Main Color Intensity", Float ) = 1
        [Toggle(NOKEWO)] _SelfLitHighContrast ("High Contrast", Float ) = 1
        _MaskSelfLit ("Mask Self Lit", 2D) = "white" {}

        _GlossIntensity ("Gloss Intensity", Range(0, 1)) = 1
        _Glossiness ("Glossiness", Range(0, 1)) = 0.6
        _GlossSoftness ("Softness", Range(0, 1)) = 0
        [HDR] _GlossColor ("Color", Color) = (1,1,1,1)
        _GlossColorPower ("Color Power", Float ) = 10
        _MaskGloss ("Mask Gloss", 2D) = "white" {}

        _GlossTexture ("Gloss Texture", 2D) = "black" {}
        _GlossTextureSoftness ("Softness", Float ) = 0
		[Toggle(NOKEWO)] _PSGLOTEX ("Pattern Style", Float ) = 0
        _GlossTextureRotate ("Rotate", Float ) = 0
        [Toggle(NOKEWO)] _GlossTextureFollowObjectRotation ("Follow Object Rotation", Float ) = 0
        _GlossTextureFollowLight ("Follow Light", Range(0, 1)) = 0

        [HDR] _OverallShadowColor ("Overall Shadow Color", Color) = (0,0,0,1)
        _OverallShadowColorPower ("Overall Shadow Color Power", Float ) = 1

        [Toggle(NOKEWO)] _SelfShadowShadowTAtViewDirection ("Self Shadow & ShadowT At View Direction", Float ) = 0

		//_ReduceShadowPointLight ("Reduce Shadow (Point Light)", float ) = 0 // Temporarily Removed
		//_PointLightSVD ("Point Light Shadow Visibility Distance", float ) = 0 /Temporarily Removed
		_ReduceShadowSpotDirectionalLight ("Reduce Shadow (Spot & Directional Light)", float ) = 0.5

		_ShadowHardness ("Shadow Hardness", Range(0, 1)) = 0

        _SelfShadowRealtimeShadowIntensity ("Self Shadow & Realtime Shadow Intensity", Range(0, 1)) = 1
        _SelfShadowThreshold ("Threshold", Range(0, 1)) = 0.85
        [Toggle(NOKEWO)] _VertexColorGreenControlSelfShadowThreshold ("Vertex Color Green Control Self Shadow Threshold", Float ) = 0
        _SelfShadowHardness ("Hardness", Range(0, 1)) = 1
        [HDR] _SelfShadowRealTimeShadowColor ("Self Shadow & Real Time Shadow Color", Color) = (1,1,1,1)
        _SelfShadowRealTimeShadowColorPower ("Self Shadow & Real Time Shadow Color Power", Float ) = 1
		[Toggle(NOKEWO)] _SelfShadowAffectedByLightShadowStrength ("Self Shadow Affected By Light Shadow Strength", Float ) = 0

        _SmoothObjectNormal ("Smooth Object Normal", Range(0, 1)) = 0
        [Toggle(NOKEWO)] _VertexColorRedControlSmoothObjectNormal ("Vertex Color Red Control Smooth Object Normal", Float ) = 0
        _XYZPosition ("XYZ Position", Vector) = (0,0,0,0)
        _XYZHardness ("XYZ Hardness", Float ) = 14
        [Toggle(NOKEWO)] _ShowNormal ("Show Normal", Float ) = 0

        _ShadowColorTexture ("Shadow Color Texture", 2D) = "white" {}
        _ShadowColorTexturePower ("Power", Float ) = 0

        _ShadowTIntensity ("ShadowT Intensity", Range(0, 1)) = 1
        _ShadowT ("ShadowT", 2D) = "white" {}
        _ShadowTLightThreshold ("Light Threshold", Float ) = 50
        _ShadowTShadowThreshold ("Shadow Threshold", Float ) = 0
		_ShadowTHardness ("Hardness", Range(0, 1)) = 1
        [HDR] _ShadowTColor ("Color", Color) = (1,1,1,1)
        _ShadowTColorPower ("Color Power", Float ) = 1

		[Toggle(NOKEWO)] _STIL ("Ignore Light", Float ) = 0

		[Toggle(N_F_STIS_ON)] _N_F_STIS ("Show In Shadow", Float ) = 0

		[Toggle(N_F_STIAL_ON )] _N_F_STIAL ("Show In Ambient Light", Float ) = 0
        _ShowInAmbientLightShadowIntensity ("Show In Ambient Light & Shadow Intensity", Range(0, 1)) = 1
        _ShowInAmbientLightShadowThreshold ("Show In Ambient Light & Shadow Threshold", Float ) = 0.4

        [Toggle(NOKEWO)] _LightFalloffAffectShadowT ("Light Falloff Affect ShadowT", Float ) = 0

        _PTexture ("PTexture", 2D) = "white" {}
        _PTexturePower ("Power", Float ) = 1

		[Toggle(N_F_RELGI_ON)] _RELG ("Receive Environmental Lighting and GI", Float ) = 1
        _EnvironmentalLightingIntensity ("Environmental Lighting Intensity", Float ) = 1

        [Toggle(NOKEWO)] _GIFlatShade ("GI Flat Shade", Float ) = 0
        _GIShadeThreshold ("GI Shade Threshold", Range(0, 1)) = 0

        [Toggle(NOKEWO)] _LightAffectShadow ("Light Affect Shadow", Float ) = 0
        _LightIntensity ("Light Intensity", Float ) = -1

		[Toggle(N_F_USETLB_ON)] _UseTLB ("Use Traditional Light Blend", Float ) = 0 
		[Toggle(N_F_EAL_ON)] _N_F_EAL ("Enable Additional Lights", Float ) = 1

		_DirectionalLightIntensity ("Directional Light Intensity", Float ) = 0
		_PointSpotlightIntensity ("Point and Spot Light Intensity", Float ) = 0
		_LightFalloffSoftness ("Light Falloff Softness", Range(0, 1)) = 1

        _CustomLightDirectionIntensity ("Intensity", Range(0, 1)) = 0
        [Toggle(NOKEWO)] _CustomLightDirectionFollowObjectRotation ("Follow Object Rotation", Float ) = 0
        _CustomLightDirection ("Custom Light Direction", Vector) = (0,0,10,0)

        _ReflectionIntensity ("Intensity", Range(0, 1)) = 0
        _ReflectionRoughtness ("Roughtness", Float ) = 0
		_RefMetallic ("Metallic", Range(0, 1) ) = 0

        _MaskReflection ("Mask Reflection", 2D) = "white" {}

        _FReflection ("FReflection", 2D) = "black" {}

        _RimLightUnfill ("Unfill", Float ) = 1.5
        [HDR] _RimLightColor ("Color", Color) = (1,1,1,1)
        _RimLightColorPower ("Color Power", Float ) = 10
        _RimLightSoftness ("Softness", Range(0, 1)) = 1
        [Toggle(NOKEWO)] _RimLightInLight ("Rim Light In Light", Float ) = 1
        [Toggle(NOKEWO)] _LightAffectRimLightColor ("Light Affect Rim Light Color", Float ) = 0

		_RefVal ("ID", int ) = 0
        [Enum(Blank,8,A,0,B,2)] _Oper("Set 1", int) = 0
        [Enum(Blank,8,None,4,A,6,B,7)] _Compa("Set 2", int) = 4

		[Toggle(N_F_MC_ON)] _N_F_MC ("MatCap", Float ) = 0
		[Toggle(N_F_NM_ON)] _N_F_NM ("Normal Map", Float ) = 0
		[Toggle(N_F_CO_ON)] _N_F_CO ("Cutout", Float ) = 0
		[Toggle(N_F_O_ON)] _N_F_O ("Outline", Float ) = 1
		[Toggle(N_F_CA_ON)] _N_F_CA ("Color Adjustment", Float ) = 0
		[Toggle(N_F_SL_ON)] _N_F_SL ("Self Lit", Float ) = 0
		[Toggle(N_F_GLO_ON)] _N_F_GLO ("Gloss", Float ) = 0
		[Toggle(N_F_GLOT_ON)] _N_F_GLOT ("Gloss Texture", Float ) = 0
		[Toggle(N_F_SS_ON)] _N_F_SS ("Self Shadow", Float ) = 1
		[Toggle(N_F_SON_ON)] _N_F_SON ("Smooth Object Normal", Float ) = 0
		[Toggle(N_F_SCT_ON)] _N_F_SCT ("Shadow Color Texture", Float ) = 0
		[Toggle(N_F_ST_ON)] _N_F_ST ("ShadowT", Float ) = 0
		[Toggle(N_F_PT_ON)] _N_F_PT ("PTexture", Float ) = 0
		[Toggle(N_F_CLD_ON)] _N_F_CLD ("Custom Light Direction", Float ) = 0
		[Toggle(N_F_R_ON)] _N_F_R ("Relfection", Float ) = 0
		[Toggle(N_F_FR_ON)] _N_F_FR ("FRelfection", Float ) = 0
		[Toggle(N_F_RL_ON)] _N_F_RL ("Rim Light", Float ) = 0

		[Toggle(N_F_HDLS_ON)] _N_F_HDLS ("Hide Directional Light Shadow", Float ) = 0
		[Toggle(N_F_HPSS_ON)] _N_F_HPSS ("Hide Point & Spot Light Shadow", Float ) = 0 

		[Toggle(N_F_DCS_ON)] _N_F_DCS ("Disable Cast Shadow", Float ) = 0
		[Toggle(N_F_NLASOBF_ON)] _N_F_NLASOBF ("No Light and Shadow On BackFace", Float ) = 0


    }

    SubShader
    {

        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
        LOD 300

		Pass {
            Name "Outline"
            Tags {
            }

            Cull [_DoubleSidedOutline]

			Stencil {
            	Ref[_RefVal]
            	Comp [_Compa]
            	Pass [_Oper]
            	Fail [_Oper]
            }

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma only_renderers d3d9 d3d11 vulkan glcore gles3 gles metal xboxone ps4 wiiu switch
            #pragma target 3.0

			#pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

			#include "Assets/RealToon/RealToon Shaders/RealToon Core/URP/RT_URP_PROP.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#pragma shader_feature_local N_F_CO_ON
			#pragma shader_feature_local N_F_EAL_ON
			#pragma shader_feature_local N_F_O_ON

			struct Attributes
            {

#if N_F_O_ON

                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 uv           : TEXCOORD0;
				float4 vertexColor	: COLOR;
				float2 uvLM         : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID

#endif

            };

            struct Varyings
            {

#if N_F_O_ON

                float2 uv                       : TEXCOORD0;
                float4 positionWSAndFogFactor   : TEXCOORD2; 
				float4 projPos					: TEXCOORD7;
				float4 vertexColor				: COLOR;
                float4 positionCS               : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO

#endif

            };


			Varyings LitPassVertex(Attributes input)
            {

			Varyings output = (Varyings)0;

#if N_F_O_ON

				UNITY_SETUP_INSTANCE_ID (input);
				UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.uv = input.uv;
                output.vertexColor = input.vertexColor;

				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

				float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );

				half RTD_OB_VP_CAL = distance(objPos.rgb,_WorldSpaceCameraPos);

				half RTD_OL_VCRAOW_OO = lerp( _OutlineWidth, (_OutlineWidth*(1.0 - output.vertexColor.b)), _VertexColorBlueAffectOutlineWitdh );
				half RTD_OL_OLWABVD_OO = lerp( RTD_OL_VCRAOW_OO, ( clamp(RTD_OL_VCRAOW_OO*RTD_OB_VP_CAL, RTD_OL_VCRAOW_OO, _FarDistanceMaxWidth) ), _OutlineWidthAffectedByViewDistance );

				//#if defined(SHADER_API_GLES)
					//half4 _OutlineWidthControl_var = tex2Dlod(_OutlineWidthControl, float4(TRANSFORM_TEX(output.uv, _OutlineWidthControl), 0.0, 0));
				//#else
					half4 _OutlineWidthControl_var = SAMPLE_TEXTURE2D_LOD(_OutlineWidthControl, sampler_OutlineWidthControl, TRANSFORM_TEX(output.uv, _OutlineWidthControl), 0);
				//#endif

                float4 node_3726 = _Time;
                float node_8530_ang = node_3726.g;
                float node_8530_spd = 0.002;
                float node_8530_cos = cos(node_8530_spd*node_8530_ang);
                float node_8530_sin = sin(node_8530_spd*node_8530_ang);
                float2 node_8530_piv = float2(0.5,0.5);
                half2 node_8530 = (mul(output.uv-node_8530_piv,float2x2( node_8530_cos, -node_8530_sin, node_8530_sin, node_8530_cos))+node_8530_piv);

				half2 RTD_OL_DNOL_OO = lerp( output.uv, node_8530, _DynamicNoisyOutline );
				half2 node_8743 = RTD_OL_DNOL_OO;

                float2 node_1283_skew = node_8743 + 0.2127+node_8743.x*0.3713*node_8743.y;
                float2 node_1283_rnd = 4.789*sin(489.123*(node_1283_skew));
                half node_1283 = frac(node_1283_rnd.x*node_1283_rnd.y*(1+node_1283_skew.x));

				_OEM = lerp(input.normalOS.xyz, normalize(input.positionOS.xyz), _OutlineExtrudeMethod);

				half RTD_OL = ( RTD_OL_OLWABVD_OO*0.01 )*_OutlineWidthControl_var.r*lerp(1.0,node_1283,_NoisyOutlineIntensity);
                output.positionCS = mul(GetWorldToHClipMatrix(), mul(GetObjectToWorldMatrix(), float4( (input.positionOS.xyz + _OutlineOffset.xyz * 0.01) + _OEM * RTD_OL,1) ) );

				#if defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)
					output.positionCS.z += _OutlineZPostionInCamera * 0.0005;
				#else
					output.positionCS.z -= _OutlineZPostionInCamera * 0.0005;
				#endif

                output.projPos = ComputeScreenPos (output.positionCS);
				float fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
				output.positionWSAndFogFactor = float4(vertexInput.positionWS, fogFactor);

#endif

                return output;

            }

            half4 LitPassFragment(Varyings input) : SV_Target
            {
#if N_F_O_ON

				UNITY_SETUP_INSTANCE_ID (input);

                float3 positionWS = input.positionWSAndFogFactor.xyz;

                Light mainLight = GetMainLight();
				half3 color = 1;

				float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float2 sceneUVs = (input.projPos.xy / input.projPos.w);

				half RTD_OB_VP_CAL = distance(objPos.rgb,_WorldSpaceCameraPos);
				half2 RTD_VD_Cal = (float2((sceneUVs.x * 2 - 1)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2 - 1).rg*RTD_OB_VP_CAL);

				half2 RTD_TC_TP_OO = lerp( input.uv, RTD_VD_Cal, _TexturePatternStyle );
				half2 node_2104 = RTD_TC_TP_OO;

				half4 _MainTex_var = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, TRANSFORM_TEX(node_2104, _MainTex));

				#if N_F_CO_ON

					half4 _SecondaryCutout_var = SAMPLE_TEXTURE2D(_SecondaryCutout, sampler_SecondaryCutout, TRANSFORM_TEX(input.uv, _SecondaryCutout));
					half RTD_CO_ON = lerp( (lerp((_MainTex_var.r*_SecondaryCutout_var.r),_SecondaryCutout_var.r,_UseSecondaryCutout)+lerp(0.5,(-1.0),_Cutout)), saturate(( (1.0 - _Cutout) > 0.5 ? (1.0-(1.0-2.0*((1.0 - _Cutout)-0.5))*(1.0-lerp((_MainTex_var.a*_SecondaryCutout_var.r),_SecondaryCutout_var.a,_UseSecondaryCutout))) : (2.0*(1.0 - _Cutout)*lerp((_MainTex_var.a*_SecondaryCutout_var.r),_SecondaryCutout_var.a,_UseSecondaryCutout)) )), _AlphaBaseCutout );
					half RTD_CO = RTD_CO_ON;
            
					#else
            
					half RTD_TC_ETT_OO = lerp( 1.0, _MainTex_var.a, _EnableTextureTransparent );
					half RTD_CO = RTD_TC_ETT_OO;

				#endif

				clip(RTD_CO - 0.5);

				float3 lightColor = mainLight.color.rgb;

#ifdef _ADDITIONAL_LIGHTS

#if N_F_EAL_ON

                int additionalLightsCount = GetAdditionalLightsCount();
                for (int i = 0; i < additionalLightsCount; ++i)
                {
                    Light light = GetAdditionalLight(i, positionWS);
					lightColor += light.color * light.distanceAttenuation;
				}
#endif

#endif
                float fogFactor = input.positionWSAndFogFactor.w;


				//
				#ifdef UNITY_COLORSPACE_GAMMA
					_OutlineColor = float4(LinearToGamma22(_OutlineColor.rgb), _OutlineColor.a);
				#endif

				float node_6587 = 0.0;
				half3 RTD_OL_LAOC_OO = lerp( lerp(_OutlineColor.rgb,_OutlineColor.rgb * _MainTex_var.rgb, _MixMainTexToOutline) , lerp(float3(node_6587,node_6587,node_6587), lerp(_OutlineColor.rgb,_OutlineColor.rgb * _MainTex_var.rgb, _MixMainTexToOutline) ,lightColor.rgb), _LightAffectOutlineColor ); //5.0.4
				//


				half3 finalRGBA = RTD_OL_LAOC_OO;

                color = MixFog(finalRGBA, fogFactor);
                return half4(color, 1);

#else

				return 0;

#endif

            }

			ENDHLSL
        }

        Pass
        {

            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}

            Cull [_DoubleSided]

			Stencil {
            	Ref[_RefVal]
            	Comp [_Compa]
            	Pass [_Oper]
            	Fail [_Oper]
            }

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma only_renderers d3d9 d3d11 vulkan glcore gles3 gles metal xboxone ps4 wiiu switch
            #pragma target 3.0

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            #pragma multi_compile_fog

            #pragma multi_compile_instancing

            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

			#include "Assets/RealToon/RealToon Shaders/RealToon Core/URP/RT_URP_PROP.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#pragma shader_feature_local N_F_USETLB_ON

			#pragma shader_feature_local N_F_MC_ON
			#pragma shader_feature_local N_F_NM_ON
			#pragma shader_feature_local N_F_CO_ON
			#pragma shader_feature_local N_F_SL_ON
			#pragma shader_feature_local N_F_CA_ON
			#pragma shader_feature_local N_F_GLO_ON
			#pragma shader_feature_local N_F_GLOT_ON
			#pragma shader_feature_local N_F_SS_ON
			#pragma shader_feature_local N_F_SCT_ON
			#pragma shader_feature_local N_F_ST_ON
			#pragma shader_feature_local N_F_STIS_ON
			#pragma shader_feature_local N_F_STIAL_ON 
			#pragma shader_feature_local N_F_SON_ON
			#pragma shader_feature_local N_F_PT_ON
			#pragma shader_feature_local N_F_RELGI_ON
			#pragma shader_feature_local N_F_CLD_ON
			#pragma shader_feature_local N_F_R_ON
			#pragma shader_feature_local N_F_FR_ON
			#pragma shader_feature_local N_F_RL_ON
			#pragma shader_feature_local N_F_HDLS_ON
			#pragma shader_feature_local N_F_HPSS_ON
			#pragma shader_feature_local N_F_EAL_ON
			#pragma shader_feature_local N_F_NLASOBF_ON

			half3 AL_GI( float3 N )
			{

				return SampleSH(N);

            }

			float3 Ref( half3 VR , half Mip )
			{

				float4 skyData = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0, samplerunity_SpecCube0, VR, Mip);
				return DecodeHDREnvironment(skyData, unity_SpecCube0_HDR);

            }

			half RTD_LVLC_F( float3 Light_Color_f3 )
			{

				#ifdef SHADER_API_MOBILE

					return saturate( dot( Light_Color_f3.rgb , float3(0.3,0.59,0.11) ) );

				#else

					float4 node_3149_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
					float4 node_3149_p = lerp(float4(float4(Light_Color_f3.rgb,0.0).zy, node_3149_k.wz), float4(float4(Light_Color_f3.rgb,0.0).yz, node_3149_k.xy), step(float4(Light_Color_f3.rgb,0.0).z, float4(Light_Color_f3.rgb,0.0).y));
					float4 node_3149_q = lerp(float4(node_3149_p.xyw, float4(Light_Color_f3.rgb,0.0).x), float4(float4(Light_Color_f3.rgb,0.0).x, node_3149_p.yzx), step(node_3149_p.x, float4(Light_Color_f3.rgb,0.0).x));
					float node_3149_d = node_3149_q.x - min(node_3149_q.w, node_3149_q.y);
					float node_3149_e = 1.0e-10;
					half3 node_3149 = float3(abs(node_3149_q.z + (node_3149_q.w - node_3149_q.y) / (6.0 * node_3149_d + node_3149_e)), node_3149_d / (node_3149_q.x + node_3149_e), node_3149_q.x);

					return saturate(node_3149.b);

				#endif

            }

            struct Attributes
            {

                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 uv           : TEXCOORD0;
				float4 vertexColor	: COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID

            };

            struct Varyings
            {

                float2 uv                       : TEXCOORD0;
                float4 positionWSAndFogFactor   : TEXCOORD2; 
                half3  normalWS                 : TEXCOORD3;
                half3 tangentWS                 : TEXCOORD4;
                half3 bitangentWS               : TEXCOORD5;

#if _MAIN_LIGHT_SHADOWS
                float4 shadowCoord              : TEXCOORD6; 
#endif
				float4 projPos					: TEXCOORD7;
				float4 posWorld					: TEXCOORD8;
				float4 vertexColor				: COLOR;
                float4 positionCS               : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO

            };

            Varyings LitPassVertex(Attributes input)
            {

                Varyings output = (Varyings)0;

				UNITY_SETUP_INSTANCE_ID (input);
				UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

				float fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
                output.positionWSAndFogFactor = float4(vertexInput.positionWS, fogFactor);
				
                output.normalWS = vertexNormalInput.normalWS;
                output.tangentWS = vertexNormalInput.tangentWS;
                output.bitangentWS = vertexNormalInput.bitangentWS;

                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                output.posWorld = mul(unity_ObjectToWorld, input.positionOS);
				output.uv = input.uv;
                output.vertexColor = input.vertexColor;
				output.positionCS = vertexInput.positionCS;

				output.projPos = ComputeScreenPos (output.positionCS);

#ifdef _MAIN_LIGHT_SHADOWS

                output.shadowCoord = GetShadowCoord(vertexInput);
#endif

                return output;
            }

            half4 LitPassFragment(Varyings input, float facing : VFACE) : SV_Target
            {

				UNITY_SETUP_INSTANCE_ID (input);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float3 positionWS = input.positionWSAndFogFactor.xyz;

#ifdef _MAIN_LIGHT_SHADOWS

                #if defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					Light mainLight = GetMainLight(TransformWorldToShadowCoord(input.posWorld.xyz));
				#else
					Light mainLight = GetMainLight(input.shadowCoord);
				#endif

#else
                Light mainLight = GetMainLight();
#endif

				#if N_F_NM_ON

					half3 _NormalMap_var = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, TRANSFORM_TEX(input.uv, _NormalMap)));
					float3 normalLocal = lerp(half3(0,0,1),_NormalMap_var.rgb,_NormalMapIntensity);

				#else

					float3 normalLocal = half3(0,0,1);

				#endif

				half3 color = 0;
				float3 A_L_O = 0;

				half isFrontFace = ( facing >= 0 ? 1 : 0 );
				float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
				float2 sceneUVs = (input.projPos.xy / input.projPos.w);

                input.normalWS = normalize(input.normalWS);
                float3x3 tangentTransform = float3x3( input.tangentWS, input.bitangentWS, input.normalWS);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - input.posWorld.xyz);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform ));
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );

				half RTD_OB_VP_CAL = distance(objPos.rgb,_WorldSpaceCameraPos);
				half2 RTD_VD_Cal = (float2((sceneUVs.x * 2 - 1)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2 - 1).rg*RTD_OB_VP_CAL);

				half2 RTD_TC_TP_OO = lerp( input.uv, RTD_VD_Cal, _TexturePatternStyle );
				half2 node_2104 = RTD_TC_TP_OO;

				#if N_F_MC_ON 
            
					half2 MUV = (mul( UNITY_MATRIX_V, float4(normalDirection,0) ).xyz.rgb.rg*0.5+0.5); 
					half4 _MatCap_var = SAMPLE_TEXTURE2D(_MCap, sampler_MCap, TRANSFORM_TEX(MUV, _MCap));
					half4 _MCapMask_var = SAMPLE_TEXTURE2D(_MCapMask, sampler_MCapMask, TRANSFORM_TEX(input.uv, _MCapMask));
					float3 MCapOutP = lerp( lerp(1,0, _SPECMODE), lerp( lerp(1,0, _SPECMODE) ,_MatCap_var.rgb,_MCapIntensity) ,_MCapMask_var.rgb ); 

				#else
            
				half MCapOutP = 1;

				#endif

				half4 _MainTex_var = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, TRANSFORM_TEX(node_2104, _MainTex));
				half3 _RTD_MVCOL = lerp(1, input.vertexColor.rgb, _MVCOL);


				//
				#ifdef UNITY_COLORSPACE_GAMMA
					_MainColor = float4(LinearToGamma22(_MainColor.rgb),_MainColor.a);
				#endif

				#if N_F_MC_ON 

					half3 SPECMode_Sel = lerp( (_MainColor.rgb * MCapOutP), ( _MainColor.rgb + (MCapOutP * _SPECIN) ), _SPECMODE);
					half3 RTD_TEX_COL = _MainTex_var.rgb * SPECMode_Sel * _RTD_MVCOL;

				#else

					half3 RTD_TEX_COL = _MainTex_var.rgb * _MainColor.rgb * MCapOutP * _RTD_MVCOL;

				#endif
				//


				#if N_F_CO_ON

					half4 _SecondaryCutout_var = SAMPLE_TEXTURE2D(_SecondaryCutout, sampler_SecondaryCutout, TRANSFORM_TEX(input.uv, _SecondaryCutout));
					half RTD_CO_ON = lerp( (lerp((_MainTex_var.r*_SecondaryCutout_var.r),_SecondaryCutout_var.r,_UseSecondaryCutout)+lerp(0.5,(-1.0),_Cutout)), saturate(( (1.0 - _Cutout) > 0.5 ? (1.0-(1.0-2.0*((1.0 - _Cutout)-0.5))*(1.0-lerp((_MainTex_var.a*_SecondaryCutout_var.r),_SecondaryCutout_var.a,_UseSecondaryCutout))) : (2.0*(1.0 - _Cutout)*lerp((_MainTex_var.a*_SecondaryCutout_var.r),_SecondaryCutout_var.a,_UseSecondaryCutout)) )), _AlphaBaseCutout );
					half RTD_CO = RTD_CO_ON;
            
				#else
            
					half RTD_TC_ETT_OO = lerp( 1.0, _MainTex_var.a, _EnableTextureTransparent );
					half RTD_CO = RTD_TC_ETT_OO;

				#endif

				clip(RTD_CO - 0.5);

				float3 lightDirection = mainLight.direction;

				#if N_F_NLASOBF_ON
					float3 lightColor = lerp(0,mainLight.color.rgb,isFrontFace);
				#else
					float3 lightColor = mainLight.color.rgb;
				#endif

                float3 halfDirection = normalize(viewDirection + lightDirection);


				#if N_F_HDLS_ON

					float attenuation = 1; 
				#else
					half dlshmin = lerp(0,0.6,_ShadowHardness);
					half dlshmax = lerp(1,0.6,_ShadowHardness);

					#if N_F_NLASOBF_ON
						half FB_Check = lerp(1,mainLight.shadowAttenuation,isFrontFace);
					#else
						half FB_Check = mainLight.shadowAttenuation;
					#endif

					half attenuation = smoothstep(dlshmin,dlshmax,FB_Check);
				#endif

				#if N_F_SON_ON

					float3 node_76 = mul( unity_WorldToObject, float4((input.posWorld.rgb-objPos.rgb),0) ).xyz.rgb.rgb;

					half RTD_SON_VCBCSON_OO = lerp( _SmoothObjectNormal, (_SmoothObjectNormal*(1.0 - input.vertexColor.r)), _VertexColorRedControlSmoothObjectNormal );
					half3 RTD_SON_ON_OTHERS = lerp(normalDirection,mul( unity_ObjectToWorld, float4(float3((_XYZPosition.r+(_XYZHardness*node_76.r)),(_XYZPosition.g+(_XYZHardness*node_76.g)),(_XYZPosition.b+(_XYZHardness*node_76.b))),0) ).xyz.rgb,RTD_SON_VCBCSON_OO);

					half3 RTD_SON = RTD_SON_ON_OTHERS;

					half3 RTD_SNorm_OO = lerp( 1.0, RTD_SON_ON_OTHERS, _ShowNormal );
					half3 RTD_SON_CHE_1 = RTD_SNorm_OO;
            
				#else
            
					half3 RTD_SON = normalDirection;
					half3 RTD_SON_CHE_1 = 1;
            
				#endif

				#if N_F_RELGI_ON

					half3 RTD_GI_ST_Sli = (RTD_SON*_GIShadeThreshold);

					float node_2183 = 0;
					float node_8383 = 0.01;
					half3 RTD_GI_FS_OO = lerp( RTD_GI_ST_Sli, float3(smoothstep( float2(node_2183,node_2183), float2(node_8383,node_8383), (RTD_SON.rb*_GIShadeThreshold) ),0.0), _GIFlatShade );

				#else

					half3 RTD_GI_FS_OO = RTD_SON;

				#endif

				#if N_F_SCT_ON
            	
					half4 _ShadowColorTexture_var = SAMPLE_TEXTURE2D(_ShadowColorTexture, sampler_ShadowColorTexture, TRANSFORM_TEX(input.uv, _ShadowColorTexture));
					half3 RTD_SCT_ON = lerp(_ShadowColorTexture_var.rgb,(_ShadowColorTexture_var.rgb*_ShadowColorTexture_var.rgb),_ShadowColorTexturePower);

					half3 RTD_SCT = RTD_SCT_ON;
            
				#else
            
					half3 RTD_SCT = 1;
            
				#endif

				#if N_F_PT_ON

					half2 node_953 = RTD_VD_Cal;
					half4 _PTexture_var = SAMPLE_TEXTURE2D(_PTexture, sampler_PTexture, TRANSFORM_TEX(node_953, _PTexture));
					half RTD_PT_ON = lerp((1.0 - _PTexturePower),1.0,_PTexture_var.r);
            
					half RTD_PT = RTD_PT_ON;
            
				#else
            
					half RTD_PT = 1;
            
				#endif


				//
				#ifdef UNITY_COLORSPACE_GAMMA
					_OverallShadowColor = float4(LinearToGamma22(_OverallShadowColor.rgb), _OverallShadowColor.a);
				#endif

				half3 RTD_OSC = (_OverallShadowColor.rgb*_OverallShadowColorPower);
				//


				//
				#ifdef UNITY_COLORSPACE_GAMMA
					_SelfShadowRealTimeShadowColor = float4(LinearToGamma22(_SelfShadowRealTimeShadowColor.rgb), _SelfShadowRealTimeShadowColor.a);
				#endif

				half3 node_1860 = ((_SelfShadowRealTimeShadowColor.rgb*_SelfShadowRealTimeShadowColorPower)*RTD_OSC*RTD_SCT*RTD_PT);
				//


                half3 node_6588 = (_LightIntensity+lightColor.rgb);

				half3 RTD_LAS = lerp(node_1860,(node_1860+node_6588),_LightAffectShadow);


				//
				#ifdef UNITY_COLORSPACE_GAMMA
					_HighlightColor = float4(LinearToGamma22(_HighlightColor.rgb), _HighlightColor.a);
				#endif

				half3 RTD_HL = (_HighlightColor.rgb*_HighlightColorPower+_DirectionalLightIntensity);
				//


				half RTD_LVLC = RTD_LVLC_F(lightColor.rgb);
				half3 RTD_MCIALO = lerp(RTD_TEX_COL , lerp(RTD_TEX_COL , _MainTex_var.rgb * MCapOutP * 0.7 , clamp((RTD_LVLC*1),0,1) ) , _MCIALO );

				#if N_F_GLO_ON

					#if N_F_GLOT_ON

						#ifndef SHADER_API_MOBILE
							float node_5992_ang = _GlossTextureRotate;
							float node_5992_spd = 1.0;
							float node_5992_cos = cos(node_5992_spd*node_5992_ang);
							float node_5992_sin = sin(node_5992_spd*node_5992_ang);
							float2 node_5992_piv = float2(0.5,0.5);
						#endif

							half3 RTD_GT_FL_Sli = lerp(viewDirection,halfDirection,_GlossTextureFollowLight);
							half3 node_2832 = reflect(RTD_GT_FL_Sli,normalDirection);

							half3 RTD_GT_FOR_OO = lerp( node_2832, mul( unity_WorldToObject, float4(node_2832,0) ).xyz.rgb, _GlossTextureFollowObjectRotation );
							half2 node_9280 = RTD_GT_FOR_OO.rg;

						#ifndef SHADER_API_MOBILE
							half2 node_5992 = (mul(float2((-1*node_9280.r),node_9280.g)-node_5992_piv,float2x2( node_5992_cos, -node_5992_sin, node_5992_sin, node_5992_cos))+node_5992_piv);
							half2 node_8759 = (node_5992*0.5+0.5);
						#endif

						#ifdef SHADER_API_MOBILE
							half4 _GlossTexture_var = SAMPLE_TEXTURE2D_LOD(_GlossTexture, sampler_GlossTexture, TRANSFORM_TEX(lerp((float2((-1 * node_2832.r), node_2832.g) * 0.5 + 0.5), RTD_VD_Cal, _PSGLOTEX), _GlossTexture), _GlossTextureSoftness);
						#else
            				half4 _GlossTexture_var = SAMPLE_TEXTURE2D_LOD(_GlossTexture, sampler_GlossTexture, TRANSFORM_TEX(lerp(node_8759, RTD_VD_Cal, _PSGLOTEX), _GlossTexture), _GlossTextureSoftness);
						#endif

						half RTD_GT_ON = _GlossTexture_var.r;

						half3 RTD_GT = RTD_GT_ON;
            
					#else

						half RTD_GLO_MAIN_Sof_Sli = lerp(0.1,1.0,_GlossSoftness);
						half RTD_NDOTH = saturate(dot(halfDirection, normalDirection));
						half RTD_GLO_MAIN = smoothstep( 0.1, RTD_GLO_MAIN_Sof_Sli, pow(RTD_NDOTH,exp2(lerp(-2,15,_Glossiness))) );

						half3 RTD_GT = RTD_GLO_MAIN;
            
					#endif

					float node_1533 = 0.0;
					half3 RTD_GLO_I_Sli = lerp(float3(node_1533,node_1533,node_1533), RTD_GT,_GlossIntensity);
					half4 _MaskGloss_var = SAMPLE_TEXTURE2D(_MaskGloss, sampler_MaskGloss, TRANSFORM_TEX(input.uv, _MaskGloss));


					//
					#ifdef UNITY_COLORSPACE_GAMMA
						_GlossColor = float4(LinearToGamma22(_GlossColor.rgb), _GlossColor.a);
					#endif

					half3 RTD_GLO_MAS = lerp( RTD_HL, lerp( RTD_HL,(_GlossColor.rgb*_GlossColorPower), RTD_GLO_I_Sli ),_MaskGloss_var.r);
					//


					half3 RTD_GLO = RTD_GLO_MAS;
            
				#else
            
					half3 RTD_GLO = RTD_HL;
            
				#endif


				half3 RTD_GLO_OTHERS = RTD_GLO;

				#if N_F_RL_ON

					float node_4353 = 0.0;
					float node_3687 = 0.0;


					//
					#ifdef UNITY_COLORSPACE_GAMMA
						_RimLightColor = float4(LinearToGamma22(_RimLightColor.rgb), _RimLightColor.a);
					#endif

            		half3 RTD_RL_LARL_OO = lerp( _RimLightColor.rgb, lerp(float3(node_3687,node_3687,node_3687),_RimLightColor.rgb,lightColor.rgb), _LightAffectRimLightColor );
					//


					half RTD_RL_S_Sli = lerp(1.70,0.29,_RimLightSoftness);
					half3 RTD_RL_MAIN = lerp(float3(node_4353,node_4353,node_4353),(RTD_RL_LARL_OO *_RimLightColorPower),smoothstep( 1.71, RTD_RL_S_Sli, pow(abs( 1.0-max(0,dot(normalDirection, viewDirection)) ),(1.0 - _RimLightUnfill)) ));
					half3 RTD_RL_IL_OO = lerp(RTD_GLO_OTHERS,(RTD_GLO_OTHERS+RTD_RL_MAIN),_RimLightInLight);

					half3 RTD_RL_CHE_1 = RTD_RL_IL_OO;
            
				#else
            
					half3 RTD_RL_CHE_1 = RTD_GLO_OTHERS;
            
				#endif

				#if N_F_CLD_ON

            		half3 RTD_CLD_CLDFOR_OO = lerp( _CustomLightDirection.rgb, mul( unity_ObjectToWorld, float4(_CustomLightDirection.rgb,0) ).xyz.rgb, _CustomLightDirectionFollowObjectRotation );
					half3 RTD_CLD_CLDI_Sli = lerp(lightDirection,RTD_CLD_CLDFOR_OO,_CustomLightDirectionIntensity); 
					half3 RTD_CLD = RTD_CLD_CLDI_Sli;
            
				#else
            
					half3 RTD_CLD = lightDirection;
            
				#endif

				half3 RTD_ST_SS_AVD_OO = lerp( RTD_CLD, viewDirection, _SelfShadowShadowTAtViewDirection );
				half RTD_NDOTL = 0.5*dot(RTD_ST_SS_AVD_OO,RTD_SON)+0.5;

				half3 RTD_ST_OFF_OTHERS = (RTD_RL_CHE_1*RTD_SON_CHE_1*lightColor.rgb);

				#if N_F_ST_ON
				
					float node_8675 = 1.0;
					half node_949 = 1.0;
					float node_5738 = 1.0;
					half node_3187 = 0.22;

					half4 _ShadowT_var = SAMPLE_TEXTURE2D(_ShadowT, sampler_ShadowT, TRANSFORM_TEX(input.uv, _ShadowT));


					//
					#ifdef UNITY_COLORSPACE_GAMMA
						_ShadowTColor = float4(LinearToGamma22(_ShadowTColor.rgb), _ShadowTColor.a);
					#endif

					half3 node_338 = ((_ShadowTColor.rgb*_ShadowTColorPower)*RTD_SCT*RTD_PT*RTD_OSC);
					//


					half RTD_ST_H_Sli = lerp(0.0,0.22,_ShadowTHardness);

					half3 RTD_ST_IS_ON = lerp(node_338,float3(node_5738,node_5738,node_5738),smoothstep( RTD_ST_H_Sli, node_3187, (_ShowInAmbientLightShadowThreshold*_ShadowT_var.rgb) )); 

					#if N_F_STIAL_ON

						float node_2346 = 1.0;
						half3 RTD_ST_ALI_Sli = lerp(float3(node_8675,node_8675,node_8675),RTD_ST_IS_ON,_ShowInAmbientLightShadowIntensity);
            			half3 RTD_STIAL_ON = lerp(RTD_ST_ALI_Sli,float3(node_2346,node_2346,node_2346),clamp((RTD_LVLC*8.0),0,1));

            			half3 RTD_STIAL = RTD_STIAL_ON;
            
            		#else
            
            			half3 RTD_STIAL = 1;
            
            		#endif

					#if N_F_STIS_ON
            
            			half3 RTD_ST_IS = lerp(1,RTD_ST_IS_ON,_ShowInAmbientLightShadowIntensity);
            
            		#else
            
            			half3 RTD_ST_IS = 1;
            
            		#endif

					half RTD_ST_LFAST_OO = lerp(lerp( RTD_NDOTL, (attenuation*RTD_NDOTL), _LightFalloffAffectShadowT ) , 1 , _STIL );
					half RTD_ST_In_Sli = lerp(node_949,smoothstep( RTD_ST_H_Sli, node_3187, ((_ShadowT_var.r*(1.0 - _ShadowTShadowThreshold))*(RTD_ST_LFAST_OO *_ShadowTLightThreshold*0.01)) ),_ShadowTIntensity);
					half3 RTD_ST_ON = lerp((lerp(node_338,(node_338+node_6588),_LightAffectShadow)*RTD_LVLC),RTD_ST_OFF_OTHERS,RTD_ST_In_Sli);

					half3 RTD_ST = RTD_ST_ON;
            
				#else
            
					half3 RTD_ST = RTD_ST_OFF_OTHERS;
					half3 RTD_STIAL = 1;
					half3 RTD_ST_IS = 1;
            
				#endif

				half node_5573 = 1.0;

				#if N_F_SS_ON
 
					half RTD_SS_SSH_Sil = lerp(0.3,1.0,_SelfShadowHardness);
					half RTD_SS_VCGCSSS_OO = lerp( _SelfShadowThreshold, (_SelfShadowThreshold*(1.0 - input.vertexColor.g)), _VertexColorGreenControlSelfShadowThreshold );
					half RTD_SS_SST = smoothstep( RTD_SS_SSH_Sil, 1.0, (RTD_NDOTL * lerp(7, RTD_SS_VCGCSSS_OO ,_SelfShadowThreshold)) );
					half RTD_SS_SSABLSS_OO = lerp( RTD_SS_SST, lerp(RTD_SS_SST,node_5573, (1.0 - _MainLightShadowData.x) ), _SelfShadowAffectedByLightShadowStrength );
					half RTD_SS_ON = lerp(node_5573,(RTD_SS_SSABLSS_OO*attenuation),_SelfShadowRealtimeShadowIntensity);

					half RTD_SS = RTD_SS_ON;
            
				#else
            
					half RTD_SS_OFF = lerp(node_5573,attenuation,_SelfShadowRealtimeShadowIntensity);

					half RTD_SS = RTD_SS_OFF;
            
				#endif
				
				half3 RTD_R_OFF_OTHERS = ( lerp( RTD_TEX_COL , _MainTex_var.rgb , _MCIALO)  * lerp((RTD_LAS*RTD_LVLC*RTD_ST_IS),RTD_ST,RTD_SS)); 

				#if N_F_R_ON

					half3 RTD_FR_OFF_OTHERS = Ref( viewReflectDirection , _ReflectionRoughtness );

					#if N_F_FR_ON
            
						half2 node_8431 = reflect(viewDirection,normalDirection).rg;
						half2 node_4207 = (float2(node_8431.r,(-1*node_8431.g))*0.5+0.5);
						half4 _FReflection_var = SAMPLE_TEXTURE2D_LOD(_FReflection, sampler_FReflection, TRANSFORM_TEX(node_4207, _FReflection), _ReflectionRoughtness);
						half3 RTD_FR_ON = _FReflection_var.rgb;

						half3 RTD_FR = RTD_FR_ON;
            
					#else
            
						half3 RTD_FR = RTD_FR_OFF_OTHERS;

					#endif

					half4 _MaskReflection_var = SAMPLE_TEXTURE2D(_MaskReflection, sampler_MaskReflection, TRANSFORM_TEX(input.uv, _MaskReflection));
					half3 RTD_R_MET_Sli = lerp(1,(9 * (RTD_TEX_COL - (9 * 0.005) ) ) , _RefMetallic);
					half3 RTD_R_MAS = lerp(RTD_R_OFF_OTHERS, (RTD_FR * RTD_R_MET_Sli) ,_MaskReflection_var.r);
					half3 RTD_R_ON = lerp(RTD_R_OFF_OTHERS, RTD_R_MAS ,_ReflectionIntensity);

					half3 RTD_R = RTD_R_ON;
            
				#else
            
					half3 RTD_R = RTD_R_OFF_OTHERS;
            
				#endif

				#if N_F_RELGI_ON

					float node_3622 = 0.0;
					float node_1766 = 1.0;
					half3 RTD_SL_OFF_OTHERS = (AL_GI( lerp(float3(node_3622,node_3622,node_3622),float3(node_1766,node_1766,node_1766),RTD_GI_FS_OO) )*_EnvironmentalLightingIntensity);

				#else

					half3 RTD_SL_OFF_OTHERS = 0;

				#endif

				#if N_F_SL_ON

            		half3 RTD_SL_HC_OO = lerp( 1.0, RTD_TEX_COL, _SelfLitHighContrast );
					half4 _MaskSelfLit_var = SAMPLE_TEXTURE2D(_MaskSelfLit, sampler_MaskSelfLit, TRANSFORM_TEX(input.uv, _MaskSelfLit));


					//
					#ifdef UNITY_COLORSPACE_GAMMA
						_SelfLitColor = float4(LinearToGamma22(_SelfLitColor.rgb), _SelfLitColor.a);
					#endif

					half3 RTD_SL_MAS = lerp(RTD_SL_OFF_OTHERS,((_SelfLitColor.rgb * RTD_TEX_COL * RTD_SL_HC_OO)*_SelfLitPower),_MaskSelfLit_var.r);
					//
					
					
					half3 RTD_SL_ON = lerp(RTD_SL_OFF_OTHERS,RTD_SL_MAS,_SelfLitIntensity);

					half3 RTD_SL = RTD_SL_ON;

					half3 RTD_R_SEL = lerp(RTD_R,lerp(RTD_R,RTD_TEX_COL*_TEXMCOLINT,_MaskSelfLit_var.r),_SelfLitIntensity); 
					half3 RTD_SL_CHE_1 = RTD_R_SEL;
            
				#else
            
					half3 RTD_SL = RTD_SL_OFF_OTHERS;
					half3 RTD_SL_CHE_1 = RTD_R;
            
				#endif

				#if N_F_RL_ON

            		half3 RTD_RL_ON = lerp((RTD_SL_CHE_1+RTD_RL_MAIN),RTD_SL_CHE_1,_RimLightInLight);
					half3 RTD_RL = RTD_RL_ON;
            
				#else
            
					half3 RTD_RL = RTD_SL_CHE_1;
            
				#endif

				half3 RTD_CA_OFF_OTHERS =  ((RTD_MCIALO*RTD_SL*RTD_STIAL)+RTD_RL);

				#if N_F_CA_ON
            
					half3 RTD_CA_ON = lerp(RTD_CA_OFF_OTHERS,dot(RTD_CA_OFF_OTHERS,float3(0.3,0.59,0.11)),(1.0 - _Saturation));
					half3 RTD_CA = RTD_CA_ON;
            
				#else

					half3 RTD_CA = RTD_CA_OFF_OTHERS;
            
				#endif

				float3 main_light_output = RTD_CA;
         

#ifdef _ADDITIONAL_LIGHTS

#if N_F_EAL_ON

                int additionalLightsCount = GetAdditionalLightsCount();

                for (int i = 0; i < additionalLightsCount; ++i)
                {
                    Light light = GetAdditionalLight(i, positionWS);

				float3 lightDirection = light.direction;


				#if N_F_NLASOBF_ON
					float3 lightColor = lerp(0,light.color.rgb,isFrontFace);
				#else
					float3 lightColor = light.color.rgb;
				#endif

                float3 halfDirection = normalize(viewDirection+lightDirection);

				#if N_F_HPSS_ON
					half attenuation = 1; 
				#else
					half dlshmin = lerp(0,0.6,_ShadowHardness);
					half dlshmax = lerp(1,0.6,_ShadowHardness);

					#if N_F_NLASOBF_ON
						half FB_Check = lerp(1,light.shadowAttenuation,isFrontFace);
					#else
						half FB_Check = light.shadowAttenuation;
					#endif

					half attenuation = smoothstep(dlshmin, dlshmax ,FB_Check);
				#endif

				half3 lightfos = smoothstep(0, _LightFalloffSoftness ,light.distanceAttenuation);


                half3 node_6588 = (_LightIntensity+lightColor.rgb);

				half3 RTD_LAS = lerp(node_1860,(node_1860+node_6588),_LightAffectShadow);
				half3 RTD_HL = (_HighlightColor.rgb*_HighlightColorPower+_PointSpotlightIntensity);

				half RTD_LVLC = RTD_LVLC_F(lightColor.rgb);
				half3 RTD_MCIALO = lerp(RTD_TEX_COL , lerp(RTD_TEX_COL , _MainTex_var.rgb * MCapOutP * 0.7 , clamp((RTD_LVLC*1),0,1) ) , _MCIALO ); 

				#if N_F_GLO_ON

					#if N_F_GLOT_ON

						#ifndef SHADER_API_MOBILE
							float node_5992_ang = _GlossTextureRotate;
							float node_5992_spd = 1.0;
							float node_5992_cos = cos(node_5992_spd*node_5992_ang);
							float node_5992_sin = sin(node_5992_spd*node_5992_ang);
							float2 node_5992_piv = float2(0.5,0.5);
						#endif

							half3 RTD_GT_FL_Sli = lerp(viewDirection,halfDirection,_GlossTextureFollowLight);
							half3 node_2832 = reflect(RTD_GT_FL_Sli,normalDirection);

							half3 RTD_GT_FOR_OO = lerp( node_2832, mul( unity_WorldToObject, float4(node_2832,0) ).xyz.rgb, _GlossTextureFollowObjectRotation );
							half2 node_9280 = RTD_GT_FOR_OO.rg;

						#ifndef SHADER_API_MOBILE
							half2 node_5992 = (mul(float2((-1*node_9280.r),node_9280.g)-node_5992_piv,float2x2( node_5992_cos, -node_5992_sin, node_5992_sin, node_5992_cos))+node_5992_piv);
							half2 node_8759 = (node_5992*0.5+0.5);
						#endif

						#ifdef SHADER_API_MOBILE
							half4 _GlossTexture_var = SAMPLE_TEXTURE2D_LOD(_GlossTexture, sampler_GlossTexture, TRANSFORM_TEX(lerp((float2((-1 * node_2832.r), node_2832.g) * 0.5 + 0.5), RTD_VD_Cal, _PSGLOTEX), _GlossTexture), _GlossTextureSoftness);
						#else
            				half4 _GlossTexture_var = SAMPLE_TEXTURE2D_LOD(_GlossTexture, sampler_GlossTexture, TRANSFORM_TEX(lerp(node_8759, RTD_VD_Cal, _PSGLOTEX), _GlossTexture), _GlossTextureSoftness);
						#endif

						half RTD_GT_ON = _GlossTexture_var.r;

						half3 RTD_GT = RTD_GT_ON;
            
					#else

						half RTD_GLO_MAIN_Sof_Sli = lerp(0.1,1.0,_GlossSoftness);
						half RTD_NDOTH = saturate(dot(halfDirection, normalDirection));
						half RTD_GLO_MAIN = smoothstep( 0.1, RTD_GLO_MAIN_Sof_Sli, pow(RTD_NDOTH,exp2(lerp(-2,15,_Glossiness))) );

						half3 RTD_GT = RTD_GLO_MAIN;
            
					#endif

					float node_1533 = 0.0;
					half3 RTD_GLO_I_Sli = lerp(float3(node_1533,node_1533,node_1533), RTD_GT,_GlossIntensity);
					half4 _MaskGloss_var = SAMPLE_TEXTURE2D(_MaskGloss, sampler_MaskGloss, TRANSFORM_TEX(input.uv, _MaskGloss));
					half3 RTD_GLO_MAS = lerp( RTD_HL, lerp( RTD_HL,(_GlossColor.rgb*_GlossColorPower), RTD_GLO_I_Sli ),_MaskGloss_var.r);

					half3 RTD_GLO = RTD_GLO_MAS;
            
				#else
            
					half3 RTD_GLO = RTD_HL;
            
				#endif


				half3 RTD_GLO_OTHERS = RTD_GLO;

				#if N_F_RL_ON

					float node_4353 = 0.0;
					float node_3687 = 0.0;
            		half3 RTD_RL_LARL_OO = lerp( _RimLightColor.rgb, lerp(float3(node_3687,node_3687,node_3687),_RimLightColor.rgb,lightColor.rgb), _LightAffectRimLightColor );
					half RTD_RL_S_Sli = lerp(1.70,0.29,_RimLightSoftness);
					half3 RTD_RL_MAIN = lerp(float3(node_4353,node_4353,node_4353),(RTD_RL_LARL_OO *_RimLightColorPower),smoothstep( 1.71, RTD_RL_S_Sli, pow(abs( 1.0-max(0,dot(normalDirection, viewDirection)) ),(1.0 - _RimLightUnfill)) ));
					half3 RTD_RL_IL_OO = lerp(RTD_GLO_OTHERS,(RTD_GLO_OTHERS+RTD_RL_MAIN),_RimLightInLight);

					half3 RTD_RL_CHE_1 = RTD_RL_IL_OO;
            
				#else
            
					half3 RTD_RL_CHE_1 = RTD_GLO_OTHERS;
            
				#endif

				#if N_F_CLD_ON

            		half3 RTD_CLD_CLDFOR_OO = lerp( _CustomLightDirection.rgb, mul( unity_ObjectToWorld, float4(_CustomLightDirection.rgb,0) ).xyz.rgb, _CustomLightDirectionFollowObjectRotation );
					half3 RTD_CLD_CLDI_Sli = lerp(lightDirection,RTD_CLD_CLDFOR_OO,_CustomLightDirectionIntensity); 
					half3 RTD_CLD = RTD_CLD_CLDI_Sli;
            
				#else
            
					half3 RTD_CLD = lightDirection;
            
				#endif

				half3 RTD_ST_SS_AVD_OO = lerp( RTD_CLD, viewDirection, _SelfShadowShadowTAtViewDirection );
				half RTD_NDOTL = 0.5*dot(RTD_ST_SS_AVD_OO,RTD_SON)+0.5;

				half3 RTD_ST_OFF_OTHERS = (RTD_RL_CHE_1*RTD_SON_CHE_1*lightColor.rgb);


				#if N_F_ST_ON
				
					float node_8675 = 1.0;
					half node_949 = 1.0;
					half node_3187 = 0.22;
					float node_5738 = 1.0;

					half4 _ShadowT_var = SAMPLE_TEXTURE2D(_ShadowT, sampler_ShadowT, TRANSFORM_TEX(input.uv, _ShadowT));

					half3 node_338 = ((_ShadowTColor.rgb*_ShadowTColorPower)*RTD_SCT*RTD_PT*RTD_OSC);

					half RTD_ST_H_Sli = lerp(0.0,0.22,_ShadowTHardness);

					half3 RTD_ST_IS_ON = lerp(node_338,float3(node_5738,node_5738,node_5738),smoothstep( RTD_ST_H_Sli, node_3187, (_ShowInAmbientLightShadowThreshold*_ShadowT_var.r) )); 

					#if N_F_STIAL_ON

						float node_2346 = 1.0;
						half3 RTD_ST_ALI_Sli = lerp(float3(node_8675,node_8675,node_8675),RTD_ST_IS_ON,_ShowInAmbientLightShadowIntensity);
            			half3 RTD_STIAL_ON = lerp(RTD_ST_ALI_Sli,float3(node_2346,node_2346,node_2346),clamp((RTD_LVLC*8.0),0,1));

            			half3 RTD_STIAL = RTD_STIAL_ON;
            
            		#else
            
            			half3 RTD_STIAL = 1;
            
            		#endif

					#if N_F_STIS_ON
            
            			half3 RTD_ST_IS = lerp(1,RTD_ST_IS_ON,_ShowInAmbientLightShadowIntensity);
            
            		#else
            
            			half3 RTD_ST_IS = 1;
            
            		#endif
            
					half RTD_ST_LFAST_OO = lerp(lerp( RTD_NDOTL, (lightfos.r*RTD_NDOTL), _LightFalloffAffectShadowT ) , 1 , _STIL );
					half RTD_ST_In_Sli = lerp(node_949,smoothstep( RTD_ST_H_Sli, node_3187, ((_ShadowT_var.r*(1.0 - _ShadowTShadowThreshold))*(RTD_ST_LFAST_OO *_ShadowTLightThreshold*0.01)) ),_ShadowTIntensity);
					half3 RTD_ST_ON = lerp((lerp(node_338,(node_338+node_6588),_LightAffectShadow)*RTD_LVLC),RTD_ST_OFF_OTHERS,RTD_ST_In_Sli);

					half3 RTD_ST = RTD_ST_ON;
            
				#else
            
					half3 RTD_ST = RTD_ST_OFF_OTHERS;
					half3 RTD_STIAL = 1;
					half3 RTD_ST_IS = 1;
            
				#endif

				half node_5573 = 1.0;

				#if N_F_SS_ON
 
					half RTD_SS_SSH_Sil = lerp(0.3,1.0,_SelfShadowHardness);
					half RTD_SS_VCGCSSS_OO = lerp( _SelfShadowThreshold, (_SelfShadowThreshold*(1.0 - input.vertexColor.g)), _VertexColorGreenControlSelfShadowThreshold );
					half RTD_SS_SST = smoothstep( RTD_SS_SSH_Sil, 1.0, (RTD_NDOTL * lerp(7, RTD_SS_VCGCSSS_OO ,_SelfShadowThreshold)) );
					half RTD_SS_SSABLSS_OO = lerp( RTD_SS_SST, lerp(RTD_SS_SST,node_5573, (1.0 - GetAdditionalLightShadowParams(i).x) ), _SelfShadowAffectedByLightShadowStrength );
					half RTD_SS_ON = lerp(node_5573,(RTD_SS_SSABLSS_OO*attenuation),_SelfShadowRealtimeShadowIntensity);

					half RTD_SS = RTD_SS_ON;
            
				#else
            
					half RTD_SS_OFF = lerp(node_5573,attenuation,_SelfShadowRealtimeShadowIntensity);

					half RTD_SS = RTD_SS_OFF;
            
				#endif
				
				half3 RTD_R_OFF_OTHERS = ( lerp( RTD_TEX_COL , _MainTex_var.rgb , _MCIALO)  * lerp((RTD_LAS*RTD_LVLC*RTD_ST_IS),RTD_ST,RTD_SS));

				#if N_F_R_ON

					half3 RTD_FR_OFF_OTHERS = Ref( viewReflectDirection , _ReflectionRoughtness );

					#if N_F_FR_ON
            
						half2 node_8431 = reflect(viewDirection,normalDirection).rg;
						half2 node_4207 = (float2(node_8431.r,(-1*node_8431.g))*0.5+0.5);
						half4 _FReflection_var = SAMPLE_TEXTURE2D_LOD(_FReflection, sampler_FReflection, TRANSFORM_TEX(node_4207, _FReflection), _ReflectionRoughtness);
						half3 RTD_FR_ON = _FReflection_var.rgb;

						half3 RTD_FR = RTD_FR_ON;
            
					#else
            
						half3 RTD_FR = RTD_FR_OFF_OTHERS;

					#endif

					half4 _MaskReflection_var = SAMPLE_TEXTURE2D(_MaskReflection, sampler_MaskReflection, TRANSFORM_TEX(input.uv, _MaskReflection));
					half3 RTD_R_MET_Sli = lerp(1,(9 * (RTD_TEX_COL - (9 * 0.005) ) ) , _RefMetallic);
					half3 RTD_R_MAS = lerp(RTD_R_OFF_OTHERS, (RTD_FR * RTD_R_MET_Sli) ,_MaskReflection_var.r);
					half3 RTD_R_ON = lerp(RTD_R_OFF_OTHERS, RTD_R_MAS ,_ReflectionIntensity);

					half3 RTD_R = RTD_R_ON;
            
				#else
            
					half3 RTD_R = RTD_R_OFF_OTHERS;
            
				#endif

				#if N_F_SL_ON

            		half3 RTD_SL_HC_OO = lerp( 1.0, RTD_TEX_COL, _SelfLitHighContrast );
					half4 _MaskSelfLit_var = SAMPLE_TEXTURE2D(_MaskSelfLit, sampler_MaskSelfLit, TRANSFORM_TEX(input.uv, _MaskSelfLit));
					half3 RTD_SL_MAS = lerp(RTD_SL_OFF_OTHERS,((_SelfLitColor.rgb * RTD_TEX_COL * RTD_SL_HC_OO)*_SelfLitPower),_MaskSelfLit_var.r);
					half3 RTD_SL_ON = lerp(RTD_SL_OFF_OTHERS,RTD_SL_MAS,_SelfLitIntensity);

					half3 RTD_SL = RTD_SL_ON;

					half3 RTD_R_SEL = lerp(RTD_R,lerp(RTD_R,RTD_TEX_COL*_TEXMCOLINT,_MaskSelfLit_var.r),_SelfLitIntensity); 
					half3 RTD_SL_CHE_1 = RTD_R_SEL;
            
				#else
            
					half3 RTD_SL = RTD_SL_OFF_OTHERS;
					half3 RTD_SL_CHE_1 = RTD_R;
            
				#endif

				#if N_F_RL_ON

            		half3 RTD_RL_ON = lerp((RTD_SL_CHE_1+RTD_RL_MAIN),RTD_SL_CHE_1,_RimLightInLight);
					half3 RTD_RL = RTD_RL_ON;
            
				#else
            
					half3 RTD_RL = RTD_SL_CHE_1;
            
				#endif

				half3 RTD_CA_OFF_OTHERS =  ((RTD_MCIALO*RTD_SL*RTD_STIAL)+RTD_RL);

				#if N_F_CA_ON
            
					half3 RTD_CA_ON = lerp(RTD_CA_OFF_OTHERS,dot(RTD_CA_OFF_OTHERS,float3(0.3,0.59,0.11)),(1.0 - _Saturation));
					half3 RTD_CA = RTD_CA_ON;
            
				#else

					half3 RTD_CA = RTD_CA_OFF_OTHERS;
            
				#endif

				float3 add_light_output = RTD_CA * lightfos;


				#if N_F_USETLB_ON
					A_L_O += add_light_output;
				#else
					A_L_O = max (add_light_output,A_L_O);
				#endif

                }
#endif

#endif
				#if N_F_USETLB_ON
					color = main_light_output + A_L_O;
				#else
					color = max (main_light_output,A_L_O);
				#endif

                float fogFactor = input.positionWSAndFogFactor.w;

                color = MixFog(color, fogFactor);
                return half4(color, 1);

            }

            ENDHLSL
			 
        }

        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            Cull Off

            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma only_renderers d3d9 d3d11 vulkan glcore gles3 gles metal xboxone ps4 wiiu switch 
            #pragma target 3.0

            #pragma multi_compile_instancing

			#pragma shader_feature_local N_F_CO_ON

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

			#include "Assets/RealToon/RealToon Shaders/RealToon Core/URP/RT_URP_PROP.hlsl"

			////uniform half _ReduceShadowPointLight; //Temporarily Removed
			////uniform half _PointLightSVD; //Temporarily Removed

			float4 _ShadowBias;
			float3 _LightDirection;

			struct Attributes
			{

				float4 positionOS   : POSITION;
				float3 normalOS     : NORMAL;
				float2 texcoord     : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID

			};

			struct Varyings
			{

				float2 uv           : TEXCOORD0;
				float4 positionCS   : SV_POSITION;
				float4 projPos		: TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO

			};

			float4 GetShadowPositionHClip(Attributes input)
			{

				float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
				float3 normalWS = TransformObjectToWorldDir(input.normalOS);

				float invNdotL = 1.0 - saturate(dot(_LightDirection, normalWS));
				float scale = invNdotL * _ShadowBias.y;

				positionWS = _LightDirection * _ShadowBias.xxx + positionWS;
				positionWS = normalWS * scale.xxx + positionWS;
				float4 positionCS = TransformWorldToHClip( positionWS );


			#if UNITY_REVERSED_Z
				positionCS.z = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE) + - _ReduceShadowSpotDirectionalLight * 0.01;
			#else
				positionCS.z = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE) + _ReduceShadowSpotDirectionalLight * 0.01;
			#endif

				return positionCS;

			}

			Varyings ShadowPassVertex(Attributes input)
			{

				Varyings output = (Varyings)0;
				UNITY_SETUP_INSTANCE_ID (input);
				UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				output.uv = input.texcoord;

				float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );

				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                output.positionCS = vertexInput.positionCS;
                output.projPos = ComputeScreenPos (output.positionCS);
				output.positionCS = GetShadowPositionHClip(input);

				return output;

			}

			half4 ShadowPassFragment(Varyings input) : SV_TARGET
			{

				UNITY_SETUP_INSTANCE_ID (input);

				float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float2 sceneUVs = (input.projPos.xy / input.projPos.w);

				half RTD_OB_VP_CAL = distance(objPos.rgb,_WorldSpaceCameraPos);
				half2 RTD_VD_Cal = (float2((sceneUVs.x * 2 - 1)*(_ScreenParams.r/_ScreenParams.g), sceneUVs.y * 2 - 1).rg*RTD_OB_VP_CAL);

                half2 _TexturePatternStyle_var = lerp( input.uv, RTD_VD_Cal, _TexturePatternStyle );
                half4 _MainTex_var = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, TRANSFORM_TEX(_TexturePatternStyle_var, _MainTex));

				#if N_F_CO_ON

					half4 _SecondaryCutout_var = SAMPLE_TEXTURE2D(_SecondaryCutout, sampler_SecondaryCutout, TRANSFORM_TEX(input.uv, _SecondaryCutout));
					half RTD_CO_ON = lerp( (lerp((_MainTex_var.r*_SecondaryCutout_var.r),_SecondaryCutout_var.r,_UseSecondaryCutout)+lerp(0.5,(-1.0),_Cutout)), saturate(( (1.0 - _Cutout) > 0.5 ? (1.0-(1.0-2.0*((1.0 - _Cutout)-0.5))*(1.0-lerp((_MainTex_var.a*_SecondaryCutout_var.r),_SecondaryCutout_var.a,_UseSecondaryCutout))) : (2.0*(1.0 - _Cutout)*lerp((_MainTex_var.a*_SecondaryCutout_var.r),_SecondaryCutout_var.a,_UseSecondaryCutout)) )), _AlphaBaseCutout );
					half RTD_CO = RTD_CO_ON;
            
				#else
            
					half RTD_TC_ETT_OO = lerp( 1.0, _MainTex_var.a, _EnableTextureTransparent );
					half RTD_CO = RTD_TC_ETT_OO;

				#endif

				clip(RTD_CO - 0.5);

				return 0;
			}

            ENDHLSL
        }

		Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            Cull [_DoubleSided]

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma only_renderers d3d9 d3d11 vulkan glcore gles3 gles metal xboxone ps4 wiiu switch 
            #pragma target 3.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            #pragma multi_compile_instancing

			#include "Assets/RealToon/RealToon Shaders/RealToon Core/URP/RT_URP_PROP.hlsl"

			struct Attributes
			{
				float4 position     : POSITION;
				float2 texcoord     : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct Varyings
			{
				float2 uv           : TEXCOORD0;
				float4 positionCS   : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			Varyings DepthOnlyVertex(Attributes input)
			{
				Varyings output = (Varyings)0;
				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				output.positionCS = TransformObjectToHClip(input.position.xyz);
				return output;
			}

			half4 DepthOnlyFragment(Varyings input) : SV_TARGET
			{
				return 0;
			}


            ENDHLSL
        }

    }

    FallBack "Hidden/InternalErrorShader"

    CustomEditor "RealToonShaderGUI_URP_SRP"
}