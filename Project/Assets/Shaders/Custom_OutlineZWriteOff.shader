Shader "Custom/OutlineZWriteOff"
{
  Properties
  {
    _Diffuse ("Diffuse", Color) = (1,1,1,1)
    _OutlineCol ("OutlineCol", Color) = (1,0,0,1)
    _OutlineFactor ("OutlineFactor", Range(0, 1)) = 0.1
    _MainTex ("Base 2D", 2D) = "white" {}
    _SHLightingScale ("LightProbe influence scale", float) = 1
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "QUEUE" = "Transparent"
      }
      ZClip Off
      ZWrite Off
      Cull Front
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 UNITY_MATRIX_P;
      //uniform float4x4 unity_MatrixInvV;
      //uniform float4x4 unity_MatrixVP;
      uniform float _OutlineFactor;
      uniform float4 _OutlineCol;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
      };
      
      struct OUT_Data_Vert
      {
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 vertex :Position;
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
          tmpvar_1.zw = tmpvar_2.zw;
          float4x4 m_4;
          m_4 = mul(unity_WorldToObject, unity_MatrixInvV);
          float4 tmpvar_5;
          float4 tmpvar_6;
          float4 tmpvar_7;
          tmpvar_5.x = conv_mxt4x4_0(m_4).x;
          tmpvar_5.y = conv_mxt4x4_1(m_4).x;
          tmpvar_5.z = conv_mxt4x4_2(m_4).x;
          tmpvar_5.w = conv_mxt4x4_3(m_4).x;
          tmpvar_6.x = conv_mxt4x4_0(m_4).y;
          tmpvar_6.y = conv_mxt4x4_1(m_4).y;
          tmpvar_6.z = conv_mxt4x4_2(m_4).y;
          tmpvar_6.w = conv_mxt4x4_3(m_4).y;
          tmpvar_7.x = conv_mxt4x4_0(m_4).z;
          tmpvar_7.y = conv_mxt4x4_1(m_4).z;
          tmpvar_7.z = conv_mxt4x4_2(m_4).z;
          tmpvar_7.w = conv_mxt4x4_3(m_4).z;
          float3x3 tmpvar_8;
          tmpvar_8[0] = tmpvar_5.xyz;
          tmpvar_8[1] = tmpvar_6.xyz;
          tmpvar_8[2] = tmpvar_7.xyz;
          float2x2 tmpvar_9;
          tmpvar_9[0] = conv_mxt4x4_0(UNITY_MATRIX_P).xy;
          tmpvar_9[1] = conv_mxt4x4_1(UNITY_MATRIX_P).xy;
          tmpvar_1.xy = (tmpvar_2.xy + mul(mul(tmpvar_9, mul(tmpvar_8, in_v.normal).xy), _OutlineFactor));
          out_v.vertex = tmpvar_1;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          out_f.color = _OutlineCol;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: 
    {
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
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
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 unity_SHAr;
      //uniform float4 unity_SHAg;
      //uniform float4 unity_SHAb;
      //uniform float4 unity_SHBr;
      //uniform float4 unity_SHBg;
      //uniform float4 unity_SHBb;
      //uniform float4 unity_SHC;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      uniform float _SHLightingScale;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_COLOR :COLOR;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_COLOR :COLOR;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float2 tmpvar_1;
          float3 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          tmpvar_1 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          float3x3 tmpvar_4;
          tmpvar_4[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_4[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_4[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float4 tmpvar_5;
          tmpvar_5.w = 1;
          tmpvar_5.xyz = mul(tmpvar_4, in_v.normal);
          float4 normal_6;
          normal_6 = tmpvar_5;
          float3 res_7;
          float3 x_8;
          x_8.x = dot(unity_SHAr, normal_6);
          x_8.y = dot(unity_SHAg, normal_6);
          x_8.z = dot(unity_SHAb, normal_6);
          float3 x1_9;
          float4 tmpvar_10;
          tmpvar_10 = (normal_6.xyzz * normal_6.yzzx);
          x1_9.x = dot(unity_SHBr, tmpvar_10);
          x1_9.y = dot(unity_SHBg, tmpvar_10);
          x1_9.z = dot(unity_SHBb, tmpvar_10);
          res_7 = (x_8 + (x1_9 + (unity_SHC.xyz * ((normal_6.x * normal_6.x) - (normal_6.y * normal_6.y)))));
          float3 tmpvar_11;
          float _tmp_dvx_10 = max(((1.055 * pow(max(res_7, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_11 = float3(_tmp_dvx_10, _tmp_dvx_10, _tmp_dvx_10);
          res_7 = tmpvar_11;
          tmpvar_2 = tmpvar_11;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          out_v.xlv_TEXCOORD0 = tmpvar_1;
          out_v.xlv_COLOR = tmpvar_2;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 col_2;
          col_2.xyz = (tex2D(_MainTex, in_f.xlv_TEXCOORD0).xyz * in_f.xlv_COLOR);
          col_2.w = 1;
          tmpvar_1 = (col_2 * _SHLightingScale);
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Diffuse"
}
