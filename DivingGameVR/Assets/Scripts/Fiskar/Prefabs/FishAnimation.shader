/*
	Author: Alberto Mellado Cruz
	Date: 09/11/2017

	Comments:
	This is just a test that would depend on the 3D Model used.
	Vertex animations would allow the use of GPU Instancing,
	enabling the use of a dense amount of animated fish.
	The code may not be optimized but it was just a test
*/

Shader "FishAnimation" {
	Properties{ 
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_SpeedX("SpeedX", Range(0, 10)) = 1
		_FrequencyX("FrequencyX", Range(0, 10)) = 1
		_AmplitudeX("AmplitudeX", Range(0, 0.2)) = 1
		_SpeedY("SpeedY", Range(0, 10)) = 1
		_FrequencyY("FrequencyY", Range(0, 10)) = 1
		_AmplitudeY("AmplitudeY", Range(0, 0.2)) = 1
		_SpeedZ("SpeedZ", Range(0, 10)) = 1
		_FrequencyZ("FrequencyZ", Range(0, 10)) = 1
		_AmplitudeZ("AmplitudeZ", Range(0,  2)) = 1
		_HeadLimit("HeadLimit", Range(-2,  2)) = 0.05
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		Cull off

		Pass{

		CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"

	sampler2D _MainTex;
	float4 _MainTex_ST;

	struct Input {
		float4 vertex: POSITION;
		float2 texcoord: TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
		UNITY_VERTEX_OUTPUT_STEREO
	};

	// X AXIS

	float _SpeedX;
	float _FrequencyX;
	float _AmplitudeX;

	// Y AXIS

	float _SpeedY;
	float _FrequencyY;
	float _AmplitudeY;

	// Z AXIS

	float _SpeedZ;
	float _FrequencyZ;
	float _AmplitudeZ;

	// Head Limit (Head wont shake so much)

	float _HeadLimit;

	v2f vert(Input v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_TRANSFER_INSTANCE_ID(v, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		//Z AXIS

		v.vertex.z += sin((v.vertex.z + _Time.y * _SpeedX) * _FrequencyX) * _AmplitudeX;

		//Y AXIS

		v.vertex.y += sin((v.vertex.z + _Time.y * _SpeedY) * _FrequencyY) * _AmplitudeY;

		//X AXIS

		if (v.vertex.z > _HeadLimit)
		{
			v.vertex.x += sin((0.05 + _Time.y * _SpeedZ) * _FrequencyZ) * _AmplitudeZ * _HeadLimit;
		}
		else
		{
			v.vertex.x += sin((v.vertex.z + _Time.y * _SpeedZ) * _FrequencyZ) * _AmplitudeZ * v.vertex.z;
		}


		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
		return o;

	}

	fixed4 frag(v2f i) : SV_Target
	{
		UNITY_SETUP_INSTANCE_ID(i);
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		return tex2D(_MainTex, i.uv);
	}

		ENDCG

	}
		}
			FallBack "Diffuse"
}