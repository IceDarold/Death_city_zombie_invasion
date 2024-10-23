Shader "cgwell/Dissolution_Blend"
{
  Properties
  {
    _TintColor ("Color&Alpha", Color) = (1,1,1,1)
    _MainTex ("Diffuse Texture", 2D) = "white" {}
    _N_mask ("N_mask", float) = 0.3
    _T_mask ("T_mask", 2D) = "white" {}
    _C_BYcolor ("C_BYcolor", Color) = (1,0,0,1)
    _N_BY_QD ("N_BY_QD", float) = 3
    _N_BY_KD ("N_BY_KD", float) = 0.01
    [HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
        "SHADOWSUPPORT" = "true"
      }
      ZClip Off
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform sampler2D _T_mask;
      uniform float4 _T_mask_ST;
      uniform sampler2D _MainTex;
      uniform float4 _MainTex_ST;
      uniform float4 _TintColor;
      uniform float _N_mask;
      uniform float _N_BY_KD;
      uniform float4 _C_BYcolor;
      uniform float _N_BY_QD;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_COLOR :COLOR;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_COLOR :COLOR;
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
          out_v.xlv_COLOR = in_v.color;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 _T_mask_var_1;
          float4 _MainTex_var_2;
          float4 tmpvar_3;
          float2 P_4;
          P_4 = TRANSFORM_TEX(in_f.xlv_TEXCOORD0, _MainTex);
          tmpvar_3 = tex2D(_MainTex, P_4);
          _MainTex_var_2 = tmpvar_3;
          float tmpvar_5;
          tmpvar_5 = (in_f.xlv_COLOR.w * _N_mask);
          float4 tmpvar_6;
          float2 P_7;
          P_7 = TRANSFORM_TEX(in_f.xlv_TEXCOORD0, _T_mask);
          tmpvar_6 = tex2D(_T_mask, P_7);
          _T_mask_var_1 = tmpvar_6;
          float tmpvar_8;
          tmpvar_8 = float((tmpvar_5>=_T_mask_var_1.x));
          float tmpvar_9;
          tmpvar_9 = lerp(tmpvar_8, 1, (float((_T_mask_var_1.x>=tmpvar_5)) * tmpvar_8));
          float tmpvar_10;
          tmpvar_10 = float((tmpvar_5>=(_T_mask_var_1.x + _N_BY_KD)));
          float tmpvar_11;
          tmpvar_11 = (tmpvar_9 - lerp(tmpvar_10, 1, (float(((_T_mask_var_1.x + _N_BY_KD)>=tmpvar_5)) * tmpvar_10)));
          float4 tmpvar_12;
          tmpvar_12.xyz = (((_TintColor.xyz * _MainTex_var_2.xyz) * in_f.xlv_COLOR.xyz) + ((tmpvar_11 * _C_BYcolor.xyz) * _N_BY_QD));
          tmpvar_12.w = (_TintColor.w * (_MainTex_var_2.w * (tmpvar_9 + tmpvar_11)));
          out_f.color = tmpvar_12;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Diffuse"
}
