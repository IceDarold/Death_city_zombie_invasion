Shader "QianKai/SimpleGrabPassBlur"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _BumpAmt ("Distortion", Range(0, 128)) = 10
    _MainTex ("Tint Color (RGB)", 2D) = "white" {}
    _BumpMap ("Normalmap", 2D) = "bump" {}
    _Size ("Size", Range(0, 20)) = 1
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Opaque"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
      }
      ZClip Off
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 0
        WriteMask 0
        Pass Keep
        Fail Keep
        ZFail Keep
        PassFront Keep
        FailFront Keep
        ZFailFront Keep
        PassBack Keep
        FailBack Keep
        ZFailBack Keep
      } 
      // m_ProgramMask = 0
      
    } // end phase
    Pass // ind: 2, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "ALWAYS"
        "QUEUE" = "Transparent"
        "RenderType" = "Opaque"
      }
      ZClip Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform sampler2D _GrabTexture;
      uniform float4 _GrabTexture_TexelSize;
      uniform float _Size;
      struct appdata_t
      {
          float4 vertex :POSITION;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          tmpvar_2 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          tmpvar_1.xy = ((tmpvar_2.xy + tmpvar_2.w) * 0.5);
          tmpvar_1.zw = tmpvar_2.zw;
          out_v.vertex = tmpvar_2;
          out_v.xlv_TEXCOORD0 = tmpvar_1;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 sum_1;
          float4 tmpvar_2;
          tmpvar_2.x = (in_f.xlv_TEXCOORD0.x + ((_GrabTexture_TexelSize.x * (-4)) * _Size));
          tmpvar_2.yzw = in_f.xlv_TEXCOORD0.yzw;
          float4 tmpvar_3;
          tmpvar_3 = tex2D(_GrabTexture, tmpvar_2);
          sum_1 = (tmpvar_3 * 0.05);
          float4 tmpvar_4;
          tmpvar_4.x = (in_f.xlv_TEXCOORD0.x + ((_GrabTexture_TexelSize.x * (-3)) * _Size));
          tmpvar_4.yzw = in_f.xlv_TEXCOORD0.yzw;
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_GrabTexture, tmpvar_4);
          sum_1 = (sum_1 + (tmpvar_5 * 0.09));
          float4 tmpvar_6;
          tmpvar_6.x = (in_f.xlv_TEXCOORD0.x + ((_GrabTexture_TexelSize.x * (-2)) * _Size));
          tmpvar_6.yzw = in_f.xlv_TEXCOORD0.yzw;
          float4 tmpvar_7;
          tmpvar_7 = tex2D(_GrabTexture, tmpvar_6);
          sum_1 = (sum_1 + (tmpvar_7 * 0.12));
          float4 tmpvar_8;
          tmpvar_8.x = (in_f.xlv_TEXCOORD0.x + ((-_GrabTexture_TexelSize.x) * _Size));
          tmpvar_8.yzw = in_f.xlv_TEXCOORD0.yzw;
          float4 tmpvar_9;
          tmpvar_9 = tex2D(_GrabTexture, tmpvar_8);
          sum_1 = (sum_1 + (tmpvar_9 * 0.15));
          float4 tmpvar_10;
          tmpvar_10 = tex2D(_GrabTexture, in_f.xlv_TEXCOORD0);
          sum_1 = (sum_1 + (tmpvar_10 * 0.18));
          float4 tmpvar_11;
          tmpvar_11.x = (in_f.xlv_TEXCOORD0.x + (_GrabTexture_TexelSize.x * _Size));
          tmpvar_11.yzw = in_f.xlv_TEXCOORD0.yzw;
          float4 tmpvar_12;
          tmpvar_12 = tex2D(_GrabTexture, tmpvar_11);
          sum_1 = (sum_1 + (tmpvar_12 * 0.15));
          float4 tmpvar_13;
          tmpvar_13.x = (in_f.xlv_TEXCOORD0.x + ((_GrabTexture_TexelSize.x * 2) * _Size));
          tmpvar_13.yzw = in_f.xlv_TEXCOORD0.yzw;
          float4 tmpvar_14;
          tmpvar_14 = tex2D(_GrabTexture, tmpvar_13);
          sum_1 = (sum_1 + (tmpvar_14 * 0.12));
          float4 tmpvar_15;
          tmpvar_15.x = (in_f.xlv_TEXCOORD0.x + ((_GrabTexture_TexelSize.x * 3) * _Size));
          tmpvar_15.yzw = in_f.xlv_TEXCOORD0.yzw;
          float4 tmpvar_16;
          tmpvar_16 = tex2D(_GrabTexture, tmpvar_15);
          sum_1 = (sum_1 + (tmpvar_16 * 0.09));
          float4 tmpvar_17;
          tmpvar_17.x = (in_f.xlv_TEXCOORD0.x + ((_GrabTexture_TexelSize.x * 4) * _Size));
          tmpvar_17.yzw = in_f.xlv_TEXCOORD0.yzw;
          float4 tmpvar_18;
          tmpvar_18 = tex2D(_GrabTexture, tmpvar_17);
          sum_1 = (sum_1 + (tmpvar_18 * 0.05));
          out_f.color = sum_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: 
    {
      Tags
      { 
      }
      ZClip Off
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 0
        WriteMask 0
        Pass Keep
        Fail Keep
        ZFail Keep
        PassFront Keep
        FailFront Keep
        ZFailFront Keep
        PassBack Keep
        FailBack Keep
        ZFailBack Keep
      } 
      // m_ProgramMask = 0
      
    } // end phase
    Pass // ind: 4, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "ALWAYS"
        "QUEUE" = "Transparent"
        "RenderType" = "Opaque"
      }
      ZClip Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform sampler2D _GrabTexture;
      uniform float4 _GrabTexture_TexelSize;
      uniform float _Size;
      struct appdata_t
      {
          float4 vertex :POSITION;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          tmpvar_2 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          tmpvar_1.xy = ((tmpvar_2.xy + tmpvar_2.w) * 0.5);
          tmpvar_1.zw = tmpvar_2.zw;
          out_v.vertex = tmpvar_2;
          out_v.xlv_TEXCOORD0 = tmpvar_1;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 sum_1;
          float4 tmpvar_2;
          tmpvar_2.x = in_f.xlv_TEXCOORD0.x;
          tmpvar_2.y = (in_f.xlv_TEXCOORD0.y + ((_GrabTexture_TexelSize.y * (-4)) * _Size));
          tmpvar_2.zw = in_f.xlv_TEXCOORD0.zw;
          float4 tmpvar_3;
          tmpvar_3 = tex2D(_GrabTexture, tmpvar_2);
          sum_1 = (tmpvar_3 * 0.05);
          float4 tmpvar_4;
          tmpvar_4.x = in_f.xlv_TEXCOORD0.x;
          tmpvar_4.y = (in_f.xlv_TEXCOORD0.y + ((_GrabTexture_TexelSize.y * (-3)) * _Size));
          tmpvar_4.zw = in_f.xlv_TEXCOORD0.zw;
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_GrabTexture, tmpvar_4);
          sum_1 = (sum_1 + (tmpvar_5 * 0.09));
          float4 tmpvar_6;
          tmpvar_6.x = in_f.xlv_TEXCOORD0.x;
          tmpvar_6.y = (in_f.xlv_TEXCOORD0.y + ((_GrabTexture_TexelSize.y * (-2)) * _Size));
          tmpvar_6.zw = in_f.xlv_TEXCOORD0.zw;
          float4 tmpvar_7;
          tmpvar_7 = tex2D(_GrabTexture, tmpvar_6);
          sum_1 = (sum_1 + (tmpvar_7 * 0.12));
          float4 tmpvar_8;
          tmpvar_8.x = in_f.xlv_TEXCOORD0.x;
          tmpvar_8.y = (in_f.xlv_TEXCOORD0.y + ((-_GrabTexture_TexelSize.y) * _Size));
          tmpvar_8.zw = in_f.xlv_TEXCOORD0.zw;
          float4 tmpvar_9;
          tmpvar_9 = tex2D(_GrabTexture, tmpvar_8);
          sum_1 = (sum_1 + (tmpvar_9 * 0.15));
          float4 tmpvar_10;
          tmpvar_10 = tex2D(_GrabTexture, in_f.xlv_TEXCOORD0);
          sum_1 = (sum_1 + (tmpvar_10 * 0.18));
          float4 tmpvar_11;
          tmpvar_11.x = in_f.xlv_TEXCOORD0.x;
          tmpvar_11.y = (in_f.xlv_TEXCOORD0.y + (_GrabTexture_TexelSize.y * _Size));
          tmpvar_11.zw = in_f.xlv_TEXCOORD0.zw;
          float4 tmpvar_12;
          tmpvar_12 = tex2D(_GrabTexture, tmpvar_11);
          sum_1 = (sum_1 + (tmpvar_12 * 0.15));
          float4 tmpvar_13;
          tmpvar_13.x = in_f.xlv_TEXCOORD0.x;
          tmpvar_13.y = (in_f.xlv_TEXCOORD0.y + ((_GrabTexture_TexelSize.y * 2) * _Size));
          tmpvar_13.zw = in_f.xlv_TEXCOORD0.zw;
          float4 tmpvar_14;
          tmpvar_14 = tex2D(_GrabTexture, tmpvar_13);
          sum_1 = (sum_1 + (tmpvar_14 * 0.12));
          float4 tmpvar_15;
          tmpvar_15.x = in_f.xlv_TEXCOORD0.x;
          tmpvar_15.y = (in_f.xlv_TEXCOORD0.y + ((_GrabTexture_TexelSize.y * 3) * _Size));
          tmpvar_15.zw = in_f.xlv_TEXCOORD0.zw;
          float4 tmpvar_16;
          tmpvar_16 = tex2D(_GrabTexture, tmpvar_15);
          sum_1 = (sum_1 + (tmpvar_16 * 0.09));
          float4 tmpvar_17;
          tmpvar_17.x = in_f.xlv_TEXCOORD0.x;
          tmpvar_17.y = (in_f.xlv_TEXCOORD0.y + ((_GrabTexture_TexelSize.y * 4) * _Size));
          tmpvar_17.zw = in_f.xlv_TEXCOORD0.zw;
          float4 tmpvar_18;
          tmpvar_18 = tex2D(_GrabTexture, tmpvar_17);
          sum_1 = (sum_1 + (tmpvar_18 * 0.05));
          out_f.color = sum_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 5, name: 
    {
      Tags
      { 
      }
      ZClip Off
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 0
        WriteMask 0
        Pass Keep
        Fail Keep
        ZFail Keep
        PassFront Keep
        FailFront Keep
        ZFailFront Keep
        PassBack Keep
        FailBack Keep
        ZFailBack Keep
      } 
      // m_ProgramMask = 0
      
    } // end phase
    Pass // ind: 6, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "ALWAYS"
        "QUEUE" = "Transparent"
        "RenderType" = "Opaque"
      }
      ZClip Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _BumpMap_ST;
      uniform float4 _MainTex_ST;
      uniform float _BumpAmt;
      uniform float4 _Color;
      uniform sampler2D _GrabTexture;
      uniform float4 _GrabTexture_TexelSize;
      uniform sampler2D _BumpMap;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float2 xlv_TEXCOORD2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float2 xlv_TEXCOORD2 :TEXCOORD2;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          tmpvar_2 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          tmpvar_1.xy = ((tmpvar_2.xy + tmpvar_2.w) * 0.5);
          tmpvar_1.zw = tmpvar_2.zw;
          out_v.vertex = tmpvar_2;
          out_v.xlv_TEXCOORD0 = tmpvar_1;
          out_v.xlv_TEXCOORD1 = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          out_v.xlv_TEXCOORD2 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          tmpvar_1.zw = in_f.xlv_TEXCOORD0.zw;
          float4 tint_2;
          float4 col_3;
          float2 bump_4;
          float2 tmpvar_5;
          tmpvar_5 = ((tex2D(_BumpMap, in_f.xlv_TEXCOORD1).xyz * 2) - 1).xy;
          bump_4 = tmpvar_5;
          tmpvar_1.xy = (((bump_4 * _BumpAmt) * (_GrabTexture_TexelSize.xy * in_f.xlv_TEXCOORD0.z)) + in_f.xlv_TEXCOORD0.xy);
          float4 tmpvar_6;
          tmpvar_6 = tex2D(_GrabTexture, tmpvar_1);
          col_3 = tmpvar_6;
          float4 tmpvar_7;
          tmpvar_7 = (tex2D(_MainTex, in_f.xlv_TEXCOORD2) * _Color);
          tint_2 = tmpvar_7;
          out_f.color = (col_3 * tint_2);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
