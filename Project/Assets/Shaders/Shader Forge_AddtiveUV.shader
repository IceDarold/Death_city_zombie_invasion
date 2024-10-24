Shader "Shader Forge/AddtiveUV"
{
  Properties
  {
    _MainTex ("MainTex", 2D) = "white" {}
    _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
    _X_speed ("X_speed", float) = 1
    _Y_speed ("Y_speed", float) = 1
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
      Blend One One
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
      //uniform float4 _Time;
      uniform float4 _TimeEditor;
      uniform sampler2D _MainTex;
      uniform float4 _MainTex_ST;
      uniform float4 _TintColor;
      uniform float _X_speed;
      uniform float _Y_speed;
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
          float4 tmpvar_1;
          float4 finalRGBA_2;
          float4 _MainTex_var_3;
          float4 tmpvar_4;
          float2 P_5;
          float4 tmpvar_6;
          tmpvar_6 = (_Time + _TimeEditor);
          P_5 = ((((in_f.xlv_TEXCOORD0 + ((tmpvar_6.x * _Y_speed) * float2(0, 1))) + ((tmpvar_6.x * _X_speed) * float2(1, 0))) * _MainTex_ST.xy) + _MainTex_ST.zw);
          tmpvar_4 = tex2D(_MainTex, P_5);
          _MainTex_var_3 = tmpvar_4;
          float4 tmpvar_7;
          tmpvar_7.w = 1;
          tmpvar_7.xyz = ((_MainTex_var_3.xyz * in_f.xlv_COLOR.xyz) * (_TintColor.xyz * 2));
          finalRGBA_2 = tmpvar_7;
          tmpvar_1 = finalRGBA_2;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
