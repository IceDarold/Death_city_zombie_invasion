Shader "Particles/Alpha Blended Smoothstep Blood Lit"
{
  Properties
  {
    _Columns ("Flipbook Columns", float) = 1
    _Rows ("Flipbook Rows", float) = 1
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _ChannelMask ("Channel Mask Color", Color) = (0,0,0,1)
    _MainTex ("Particle Texture", 2D) = "white" {}
    _EdgeMin ("SmoothStep Edge Min", float) = 0.05
    _EdgeSoft ("SmoothStep Softness", float) = 0.05
    _NormalMap ("Normal Map", 2D) = "bump" {}
    _HighlightColor ("Highlight Color", Color) = (1,1,1,0)
    _Reflect ("Reflection Matcap", 2D) = "black" {}
    _Detail ("Detail Tex", 2D) = "gray" {}
    _DetailTile ("Detail Tiling", float) = 6
    _DetailPan ("Detail Alpha Pan", float) = 0.1
    _DetailAlphaAffect ("Detail Alpha Affect", float) = 1
    _DetailBrightAffect ("Detail Brightness Affect", float) = 0.5
    _UVOff ("UV Offset Map", 2D) = "bump" {}
    _OffPow ("UV Offset Power", float) = 0.1
    _OffTile ("UV Offset Tiling", float) = 1
    _Overbright ("Overbright", float) = 0
    _InvFade ("Soft Particles Factor", Range(0.01, 8)) = 3
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZClip Off
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      //uniform float4 glstate_lightmodel_ambient;
      uniform sampler2D _MainTex;
      uniform sampler2D _Detail;
      uniform sampler2D _NormalMap;
      uniform sampler2D _UVOff;
      uniform sampler2D _Reflect;
      uniform float4 _TintColor;
      uniform float4 _ChannelMask;
      uniform float4 _HighlightColor;
      uniform float _EdgeMin;
      uniform float _EdgeSoft;
      uniform float _Overbright;
      uniform float _DetailTile;
      uniform float _DetailPan;
      uniform float _OffPow;
      uniform float _OffTile;
      uniform float _DetailBrightAffect;
      uniform float _DetailAlphaAffect;
      uniform float _Columns;
      uniform float _Rows;
      uniform float4 _LightColor0;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR :COLOR;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR :COLOR;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1.w = 1;
          tmpvar_1.xyz = in_v.vertex.xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_1));
          out_v.xlv_COLOR = in_v.color;
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 highlight_1;
          float4 col_2;
          float4 tex_3;
          float4 detail_4;
          float edgeMask_5;
          float3 lightColor_6;
          float3 tmpvar_7;
          tmpvar_7 = (_LightColor0.xyz + (glstate_lightmodel_ambient * 2).xyz);
          lightColor_6 = tmpvar_7;
          float tmpvar_8;
          float tmpvar_9;
          tmpvar_9 = clamp((2 * frac((_Rows * in_f.xlv_TEXCOORD0.x))), 0, 1);
          tmpvar_8 = tmpvar_9;
          float tmpvar_10;
          tmpvar_10 = clamp((2 * frac(((-_Rows) * in_f.xlv_TEXCOORD0.x))), 0, 1);
          edgeMask_5 = (tmpvar_8 * tmpvar_10);
          float tmpvar_11;
          tmpvar_11 = clamp((2 * frac((_Columns * in_f.xlv_TEXCOORD0.y))), 0, 1);
          edgeMask_5 = (edgeMask_5 * tmpvar_11);
          float tmpvar_12;
          tmpvar_12 = clamp((2 * frac(((-_Columns) * in_f.xlv_TEXCOORD0.y))), 0, 1);
          edgeMask_5 = (edgeMask_5 * tmpvar_12);
          float2 tmpvar_13;
          tmpvar_13.x = (in_f.xlv_COLOR.y * 154.6);
          tmpvar_13.y = (in_f.xlv_COLOR.y * 798.3);
          float2 P_14;
          P_14 = ((in_f.xlv_TEXCOORD0 + tmpvar_13) * _OffTile);
          float4 tmpvar_15;
          tmpvar_15 = ((tex2D(_UVOff, P_14) * 2) - 1);
          float2 tmpvar_16;
          tmpvar_16.x = (tmpvar_15.y * _OffPow);
          tmpvar_16.y = (tmpvar_15.w * _OffPow);
          float2 tmpvar_17;
          float _tmp_dvx_8 = ((0.1 * tmpvar_16) * edgeMask_5);
          tmpvar_17 = float2(_tmp_dvx_8, _tmp_dvx_8);
          float2 P_18;
          P_18 = (in_f.xlv_TEXCOORD0 + tmpvar_17);
          float3 tmpvar_19;
          tmpvar_19 = ((((tex2D(_NormalMap, P_18).xyz * 2) - 1) * 0.5) + 0.5);
          float2 tmpvar_20;
          tmpvar_20.y = 0;
          tmpvar_20.x = ((in_f.xlv_COLOR.y * 1.32) + (in_f.xlv_COLOR.w * _DetailPan));
          float4 tmpvar_21;
          float2 P_22;
          P_22 = (((_DetailTile * in_f.xlv_TEXCOORD0) + tmpvar_20) + tmpvar_17);
          tmpvar_21 = tex2D(_Detail, P_22);
          detail_4.xyz = tmpvar_21.xyz;
          detail_4.w = lerp(1, tmpvar_21.w, _DetailAlphaAffect);
          detail_4.w = (detail_4.w * lerp(float3(1, 1, 1), tmpvar_21.xyz, float3(_DetailAlphaAffect, _DetailAlphaAffect, _DetailAlphaAffect)).x);
          float _tmp_dvx_9 = (_DetailBrightAffect + 0.5);
          detail_4.xyz = lerp((1 - tmpvar_21.xyz), tmpvar_21.xyz, float3(_tmp_dvx_9, _tmp_dvx_9, _tmp_dvx_9));
          float4 tmpvar_23;
          float2 P_24;
          P_24 = (in_f.xlv_TEXCOORD0 + tmpvar_17);
          tmpvar_23 = tex2D(_MainTex, P_24);
          tex_3.xyz = tmpvar_23.xyz;
          float4 tmpvar_25;
          tmpvar_25.w = 1;
          tmpvar_25.xyz = _TintColor.xyz;
          col_2.xyz = tmpvar_25.xyz;
          float4 x_26;
          x_26 = (tmpvar_23 * _ChannelMask);
          tex_3.w = sqrt(dot(x_26, x_26));
          col_2.w = (tex_3.w * (in_f.xlv_COLOR.w * detail_4.w));
          float4 tmpvar_27;
          tmpvar_27 = tex2D(_Reflect, lerp(tmpvar_19.xy, (tmpvar_19.xy * 1.5), detail_4.ww));
          float3 tmpvar_28;
          tmpvar_28 = ((in_f.xlv_COLOR.z * lightColor_6) * ((tmpvar_27.xyz * _HighlightColor.xyz) * 2));
          highlight_1 = tmpvar_28;
          float tmpvar_29;
          tmpvar_29 = clamp(((col_2.w - _EdgeMin) / ((_EdgeMin + _EdgeSoft) - _EdgeMin)), 0, 1);
          col_2.w = (tmpvar_29 * (tmpvar_29 * (3 - (2 * tmpvar_29))));
          col_2.xyz = (((_TintColor.xyz * detail_4.xyz) * in_f.xlv_COLOR.x) * (_Overbright + 1));
          float tmpvar_30;
          float3 x_31;
          x_31 = (highlight_1 * col_2.w);
          tmpvar_30 = sqrt(dot(x_31, x_31));
          float4 tmpvar_32;
          tmpvar_32.xyz = (highlight_1 + (col_2.xyz * lightColor_6));
          tmpvar_32.w = ((col_2.w * _TintColor.w) + (tmpvar_30 * 0.5));
          col_2 = tmpvar_32;
          out_f.color = col_2;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
