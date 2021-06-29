// Allows for anti-aliasing
// https://bgolus.medium.com/anti-aliased-alpha-test-the-esoteric-alpha-to-coverage-8b177335ae4f

Shader "BeatSaber/UI Cutout ATC"
{
	Properties {
		_MainTex("Main Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Cutoff("Cutoff", Range(0, 1)) = 0.5
		_MipScale("Mip Level Alpha Scale", Range(0, 1)) = 0.25
	}

	Category {
		Tags { "RenderQueue" = "AlphaTest" "RenderType" = "TransparentCutout" }
		Cull Off
		//Lighting Off
		//ZWrite Off

		SubShader {
			Pass {
				Tags { "LightMode" = "ForwardBase" }
				AlphaToMask On

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _MainTex_TexelSize;
				float4 _Color;
				float _Cutoff;
				float _MipScale;

				float CalcMipLevel(float2 texcoord) {
					float2 dx = ddx(texcoord);
					float2 dy = ddy(texcoord);
					float deltaMaxSqr = max(dot(dx, dx), dot(dy, dy));

					return max(0.0, 0.5 * log2(deltaMaxSqr));
				}

				struct appdata_t {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					float2 texcoord : TEXCOORD0;
					half4 color : COLOR;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
				};

				v2f vert(appdata_full v)
				{
					v2f o;

					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_OUTPUT(v2f, o);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);

					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.color = v.color;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID(i);

					float4 col = tex2D(_MainTex, i.texcoord);
					col.a *= 1 + max(0, CalcMipLevel(i.texcoord * _MainTex_TexelSize.zw)) * _MipScale;
					col.a = (col.a - _Cutoff) / max(fwidth(col.a), 0.0001) + 0.5f;

					/*if (col.a < _Cutoff)
						clip(-1);

					col.a = 0;*/

					return _Color * col * i.color;
				}
				ENDCG
			}
		}
	}
}
