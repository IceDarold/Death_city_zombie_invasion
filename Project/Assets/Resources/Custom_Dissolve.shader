// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Dissolve" {
	
	
		Properties
		{
			//外部脚本控制_Value来控制是否丢弃某个像素
			_MainTex("Texture", 2D) = "white" {}
			_DissolveTex("Texture", 2D) = "white" {}
			_DissSize("DissSize", Range(0, 1)) = 0.1 //溶解阈值，小于阈值才属于溶解带
			_DissColor("DissColor", Color) = (1,0,0,1)//溶解带的渐变颜色，与_AddColor配合形成渐变色
			_AddColor("AddColor", Color) = (1,1,0,1)
			_Value("Value", Range(0,1)) = 0.5 //这个属性其实不用开放出来的，通过脚本控制就好，但我想看效果却懒得写脚本，就这样吧
		}
			SubShader
			{
				Tags { "RenderType" = "Opaque" }
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
						float2 uv : TEXCOORD0;
					};

					struct v2f
					{
						float2 uv : TEXCOORD0;
						float4 vertex : SV_POSITION;
					};

					sampler2D _MainTex;
					sampler2D _DissolveTex;
					float4 _MainTex_ST;
					half _Value;//脚本控制的变量
					half _DissSize;
					half4 _DissColor, _AddColor;

					v2f vert(appdata v)
					{
						v2f o;
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.uv = TRANSFORM_TEX(v.uv, _MainTex);
						return o;
					}

					fixed4 frag(v2f i) : SV_Target
					{
						fixed4 col = tex2D(_MainTex, i.uv);
						float dissolveValue = tex2D(_DissolveTex, i.uv).r;
						float clipValue = dissolveValue - _Value;
						clip(clipValue);// 如果clipValue<0则抛弃当前像素，
						//clip is equivalent to (if(clipValue<0)discard;),但好像好像discard需要更高一点的硬件支持
						//也可以通过设置col.a = 0抛弃当前像素：if(clipValue<0)col.a = 0;
						if (clipValue > 0 && clipValue < _DissSize)
						{
							//溶解带渐变
							half4 dissolveColor = lerp(_DissColor, _AddColor, clipValue / _DissSize) * 2;
							col *= dissolveColor;
						}
						return col;
					}
					ENDCG
				}
			}
	}