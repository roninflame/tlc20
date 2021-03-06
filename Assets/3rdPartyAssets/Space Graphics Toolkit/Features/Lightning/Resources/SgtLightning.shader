Shader "Hidden/SgtLightning"
{
	Properties
	{
		_MainTex ("Start Fade (R) End Fade (G) Opacity (B)", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Age("Age", Float) = 0
		_Scale("Scale", Vector) = (0,0,0,0)
		_Offset("Offset", Vector) = (0,0,0,0)
	}
	SubShader
	{
		Tags
		{
			"Queue"             = "Transparent"
			"RenderType"        = "Transparent"
			"PreviewType"       = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Pass
		{
			Blend One One, One One
			Cull Off
			ZWrite Off

			CGPROGRAM
			#pragma vertex   Vert
			#pragma fragment Frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4    _Color;
			float     _Age;
			float2    _Scale;
			float2    _Offset;

			struct a2v
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float2 texcoord : TEXCOORD0;

				UNITY_VERTEX_OUTPUT_STEREO
			};

			struct f2g
			{
				float4 color : SV_TARGET;
			};

			void Vert(a2v i, out v2f o)
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.vertex   = UnityObjectToClipPos(i.vertex);
				o.texcoord = i.texcoord * _Scale + _Offset;
			}

			void Frag(v2f i, out f2g o)
			{
				float4 mainTex  = tex2D(_MainTex, i.texcoord);
				float  mainMin  = mainTex.g;
				float  mainMax  = mainTex.b;
				float  mainRng  = mainMax - mainMin;
				float  progress = saturate((_Age - mainMin) / mainRng); // 0..1

				progress = 1.0f - abs(progress * 2.0f - 1.0f);

				o.color = saturate(_Color * mainTex.r * progress);
			}
		ENDCG
		} // Pass
	} // SubShader
} // Shader
