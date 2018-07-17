/*
 * Super rough 3D Texture visualisation shader
 
 * (c) Copyright 2017, 2018, Warwick Molloy
 * GitHub repo WazzaMo/UnityComponents
 * Provided under the terms of the MIT License.
 */


Shader "UnityComponents/VolumeView"
{
	Properties
	{
		// _MainTex ("Texture", 2D) = "white" {}
		_Volume ("Texture", 3D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 uvw : TEXCOORD0;
			};

			struct v2f
			{
				float3 uvw : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			// sampler2D _MainTex;
			// float4 _MainTex_ST;

			sampler3D _Volume;
			float4 _Volume_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uvw = (o.vertex + float3(1,1,1)) / 2;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex3D(_Volume, i.uvw);
				return col;
			}
			ENDCG
		}
	}
}
