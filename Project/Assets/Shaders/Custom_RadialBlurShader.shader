Shader "Custom/RadialBlurShader"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _BlurTex ("Blur Tex", 2D) = "white" {}
  }
  SubShader
  {
    Tags
    { 
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
      }
      ZClip Off
      ZTest Always
      ZWrite Off
      Cull Off
      Fog
      { 
        Mode  Off
      } 
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform sampler2D _MainTex;
      uniform float _BlurFactor;
      uniform float4 _BlurCenter;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
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
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 outColor_2;
          float2 dir_3;
          dir_3 = (in_f.xlv_TEXCOORD0 - _BlurCenter.xy);
          float2 tmpvar_4;
          tmpvar_4 = in_f.xlv_TEXCOORD0;
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_MainTex, tmpvar_4);
          outColor_2 = tmpvar_5;
          float2 tmpvar_6;
          tmpvar_6 = (in_f.xlv_TEXCOORD0 + (_BlurFactor * dir_3));
          float4 tmpvar_7;
          tmpvar_7 = tex2D(_MainTex, tmpvar_6);
          outColor_2 = (outColor_2 + tmpvar_7);
          float2 tmpvar_8;
          tmpvar_8 = (in_f.xlv_TEXCOORD0 + ((_BlurFactor * dir_3) * 2));
          float4 tmpvar_9;
          tmpvar_9 = tex2D(_MainTex, tmpvar_8);
          outColor_2 = (outColor_2 + tmpvar_9);
          float2 tmpvar_10;
          tmpvar_10 = (in_f.xlv_TEXCOORD0 + ((_BlurFactor * dir_3) * 3));
          float4 tmpvar_11;
          tmpvar_11 = tex2D(_MainTex, tmpvar_10);
          outColor_2 = (outColor_2 + tmpvar_11);
          float2 tmpvar_12;
          tmpvar_12 = (in_f.xlv_TEXCOORD0 + ((_BlurFactor * dir_3) * 4));
          float4 tmpvar_13;
          tmpvar_13 = tex2D(_MainTex, tmpvar_12);
          outColor_2 = (outColor_2 + tmpvar_13);
          float2 tmpvar_14;
          tmpvar_14 = (in_f.xlv_TEXCOORD0 + ((_BlurFactor * dir_3) * 5));
          float4 tmpvar_15;
          tmpvar_15 = tex2D(_MainTex, tmpvar_14);
          outColor_2 = (outColor_2 + tmpvar_15);
          outColor_2 = (outColor_2 / 6);
          tmpvar_1 = outColor_2;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: 
    {
      Tags
      { 
      }
      ZClip Off
      ZTest Always
      ZWrite Off
      Cull Off
      Fog
      { 
        Mode  Off
      } 
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform sampler2D _MainTex;
      uniform sampler2D _BlurTex;
      uniform float _LerpFactor;
      uniform float4 _BlurCenter;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float2 tmpvar_1;
          tmpvar_1 = in_v.texcoord.xy;
          float2 tmpvar_2;
          float2 tmpvar_3;
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = in_v.vertex.xyz;
          tmpvar_2 = tmpvar_1;
          tmpvar_3 = tmpvar_1;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_4));
          out_v.xlv_TEXCOORD0 = tmpvar_2;
          out_v.xlv_TEXCOORD1 = tmpvar_3;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float2 tmpvar_2;
          tmpvar_2 = (in_f.xlv_TEXCOORD0 - _BlurCenter.xy);
          float4 tmpvar_3;
          tmpvar_3 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          float4 tmpvar_4;
          tmpvar_4 = tex2D(_BlurTex, in_f.xlv_TEXCOORD1);
          float4 tmpvar_5;
          float _tmp_dvx_16 = (_LerpFactor * sqrt(dot(tmpvar_2, tmpvar_2)));
          tmpvar_5 = lerp(tmpvar_3, tmpvar_4, float4(_tmp_dvx_16, _tmp_dvx_16, _tmp_dvx_16, _tmp_dvx_16));
          tmpvar_1 = tmpvar_5;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
