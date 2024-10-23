// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "Transparent/VertexLit"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _SpecColor ("Spec Color", Color) = (1,1,1,0)
    _Emission ("Emissive Color", Color) = (0,0,0,0)
    _Shininess ("Shininess", Range(0.1, 1)) = 0.7
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "Vertex"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 100
      ZClip Off
      ZWrite Off
      Fog
      { 
        Mode  Off
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
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
      //uniform float4 unity_LightColor[8];
      //uniform float4 unity_LightPosition[8];
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 glstate_lightmodel_ambient;
      //uniform float4x4 unity_MatrixV;
      //uniform float4x4 unity_MatrixInvV;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color;
      uniform float4 _SpecColor;
      uniform float4 _Emission;
      uniform float _Shininess;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR0 :COLOR0;
          float3 xlv_COLOR1 :COLOR1;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR0 :COLOR0;
          float3 xlv_COLOR1 :COLOR1;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3 tmpvar_1;
          tmpvar_1 = in_v.vertex.xyz;
          float shininess_3;
          float3 specColor_4;
          float3 lcolor_5;
          float3 viewDir_6;
          float3 eyeNormal_7;
          float4 color_8;
          color_8 = float4(0, 0, 0, 1.1);
          float4 tmpvar_9;
          tmpvar_9.w = 1;
          tmpvar_9.xyz = float3(tmpvar_1);
          float4x4 m_10;
          m_10 = mul(unity_WorldToObject, unity_MatrixInvV);
          float4 tmpvar_11;
          float4 tmpvar_12;
          float4 tmpvar_13;
          tmpvar_11.x = conv_mxt4x4_0(m_10).x;
          tmpvar_11.y = conv_mxt4x4_1(m_10).x;
          tmpvar_11.z = conv_mxt4x4_2(m_10).x;
          tmpvar_11.w = conv_mxt4x4_3(m_10).x;
          tmpvar_12.x = conv_mxt4x4_0(m_10).y;
          tmpvar_12.y = conv_mxt4x4_1(m_10).y;
          tmpvar_12.z = conv_mxt4x4_2(m_10).y;
          tmpvar_12.w = conv_mxt4x4_3(m_10).y;
          tmpvar_13.x = conv_mxt4x4_0(m_10).z;
          tmpvar_13.y = conv_mxt4x4_1(m_10).z;
          tmpvar_13.z = conv_mxt4x4_2(m_10).z;
          tmpvar_13.w = conv_mxt4x4_3(m_10).z;
          float3x3 tmpvar_14;
          tmpvar_14[0] = tmpvar_11.xyz;
          tmpvar_14[1] = tmpvar_12.xyz;
          tmpvar_14[2] = tmpvar_13.xyz;
          float3 tmpvar_15;
          tmpvar_15 = normalize(mul(tmpvar_14, in_v.normal));
          eyeNormal_7 = tmpvar_15;
          float3 tmpvar_16;
          tmpvar_16 = normalize(mul(mul(unity_MatrixV, unity_ObjectToWorld), tmpvar_9).xyz);
          viewDir_6 = (-tmpvar_16);
          lcolor_5 = (_Emission.xyz + (_Color.xyz * glstate_lightmodel_ambient.xyz));
          specColor_4 = float3(0, 0, 0);
          shininess_3 = (_Shininess * 128);
          int il_2 = 0;
          while((il_2<8))
          {
              float3 tmpvar_17;
              tmpvar_17 = unity_LightPosition[il_2].xyz;
              float3 dirToLight_18;
              dirToLight_18 = tmpvar_17;
              float3 specColor_19;
              specColor_19 = specColor_4;
              float tmpvar_20;
              tmpvar_20 = max(dot(eyeNormal_7, dirToLight_18), 0);
              float3 tmpvar_21;
              tmpvar_21 = ((tmpvar_20 * _Color.xyz) * unity_LightColor[il_2].xyz);
              if((tmpvar_20>0))
              {
                  specColor_19 = (specColor_4 + ((0.5 * clamp(pow(max(dot(eyeNormal_7, normalize((dirToLight_18 + viewDir_6))), 0), shininess_3), 0, 1)) * unity_LightColor[il_2].xyz));
              }
              specColor_4 = specColor_19;
              lcolor_5 = (lcolor_5 + min((tmpvar_21 * 0.5), float3(1, 1, 1)));
              il_2 = (il_2 + 1);
          }
          color_8.xyz = float3(lcolor_5);
          color_8.w = _Color.w;
          specColor_4 = (specColor_4 * _SpecColor.xyz);
          float4 tmpvar_22;
          float4 tmpvar_23;
          tmpvar_23 = clamp(color_8, 0, 1);
          tmpvar_22 = tmpvar_23;
          float3 tmpvar_24;
          float3 tmpvar_25;
          tmpvar_25 = clamp(specColor_4, 0, 1);
          tmpvar_24 = tmpvar_25;
          float4 tmpvar_26;
          tmpvar_26.w = 1;
          tmpvar_26.xyz = float3(tmpvar_1);
          out_v.xlv_COLOR0 = tmpvar_22;
          out_v.xlv_COLOR1 = tmpvar_24;
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_26));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 col_1;
          float4 tmpvar_2;
          tmpvar_2 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          col_1.xyz = (tmpvar_2 * in_f.xlv_COLOR0).xyz;
          col_1.xyz = (col_1 * 2).xyz;
          col_1.w = (tmpvar_2.w * in_f.xlv_COLOR0.w);
          if((col_1.w<=0))
          {
              discard;
          }
          col_1.xyz = (col_1.xyz + in_f.xlv_COLOR1);
          out_f.color = col_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "VertexLM"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 100
      ZClip Off
      ZWrite Off
      Fog
      { 
        Mode  Off
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      // uniform float4 unity_LightmapST;
      uniform float4 _MainTex_ST;
      // uniform sampler2D unity_Lightmap;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR0 :COLOR0;
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
          float4 tmpvar_1;
          float4 tmpvar_2;
          tmpvar_2 = clamp(in_v.color, 0, 1);
          tmpvar_1 = tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          out_v.xlv_COLOR0 = tmpvar_1;
          out_v.xlv_TEXCOORD0 = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
          out_v.xlv_TEXCOORD1 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tex_1;
          float4 col_2;
          float4 tmpvar_3;
          tmpvar_3 = UNITY_SAMPLE_TEX2D(unity_Lightmap, in_f.xlv_TEXCOORD0);
          tex_1 = tmpvar_3;
          col_2 = (tex_1 * _Color);
          float4 tmpvar_4;
          tmpvar_4 = tex2D(_MainTex, in_f.xlv_TEXCOORD1);
          tex_1 = tmpvar_4;
          col_2.xyz = (tmpvar_4 * col_2).xyz;
          col_2.xyz = (col_2 * 2).xyz;
          col_2.w = (tmpvar_4.w * in_f.xlv_COLOR0.w);
          if((col_2.w<=0))
          {
              discard;
          }
          out_f.color = col_2;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "VertexLMRGBM"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 100
      ZClip Off
      ZWrite Off
      Fog
      { 
        Mode  Off
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      // uniform float4 unity_LightmapST;
      uniform float4 unity_Lightmap_ST;
      uniform float4 _MainTex_ST;
      // uniform sampler2D unity_Lightmap;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float2 xlv_TEXCOORD2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
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
          tmpvar_2 = clamp(in_v.color, 0, 1);
          tmpvar_1 = tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          out_v.xlv_COLOR0 = tmpvar_1;
          out_v.xlv_TEXCOORD0 = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
          out_v.xlv_TEXCOORD1 = TRANSFORM_TEX(in_v.texcoord1.xy, unity_Lightmap);
          out_v.xlv_TEXCOORD2 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tex_1;
          float4 col_2;
          float4 tmpvar_3;
          tmpvar_3 = UNITY_SAMPLE_TEX2D(unity_Lightmap, in_f.xlv_TEXCOORD0);
          tex_1 = tmpvar_3;
          col_2 = (tex_1 * tex_1.w);
          col_2 = (col_2 * 2);
          col_2 = (col_2 * _Color);
          float4 tmpvar_4;
          tmpvar_4 = tex2D(_MainTex, in_f.xlv_TEXCOORD2);
          tex_1 = tmpvar_4;
          col_2.xyz = (tmpvar_4 * col_2).xyz;
          col_2.xyz = (col_2 * 4).xyz;
          col_2.w = (tmpvar_4.w * in_f.xlv_COLOR0.w);
          if((col_2.w<=0))
          {
              discard;
          }
          out_f.color = col_2;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "ALWAYS"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 100
      ZClip Off
      ZWrite Off
      Fog
      { 
        Mode  Off
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
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
      //uniform float4 unity_LightColor[8];
      //uniform float4 unity_LightPosition[8];
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 glstate_lightmodel_ambient;
      //uniform float4x4 unity_MatrixV;
      //uniform float4x4 unity_MatrixInvV;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color;
      uniform float4 _SpecColor;
      uniform float4 _Emission;
      uniform float _Shininess;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR0 :COLOR0;
          float3 xlv_COLOR1 :COLOR1;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR0 :COLOR0;
          float3 xlv_COLOR1 :COLOR1;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3 tmpvar_1;
          tmpvar_1 = in_v.vertex.xyz;
          float shininess_3;
          float3 specColor_4;
          float3 lcolor_5;
          float3 viewDir_6;
          float3 eyeNormal_7;
          float4 color_8;
          color_8 = float4(0, 0, 0, 1.1);
          float4 tmpvar_9;
          tmpvar_9.w = 1;
          tmpvar_9.xyz = float3(tmpvar_1);
          float4x4 m_10;
          m_10 = mul(unity_WorldToObject, unity_MatrixInvV);
          float4 tmpvar_11;
          float4 tmpvar_12;
          float4 tmpvar_13;
          tmpvar_11.x = conv_mxt4x4_0(m_10).x;
          tmpvar_11.y = conv_mxt4x4_1(m_10).x;
          tmpvar_11.z = conv_mxt4x4_2(m_10).x;
          tmpvar_11.w = conv_mxt4x4_3(m_10).x;
          tmpvar_12.x = conv_mxt4x4_0(m_10).y;
          tmpvar_12.y = conv_mxt4x4_1(m_10).y;
          tmpvar_12.z = conv_mxt4x4_2(m_10).y;
          tmpvar_12.w = conv_mxt4x4_3(m_10).y;
          tmpvar_13.x = conv_mxt4x4_0(m_10).z;
          tmpvar_13.y = conv_mxt4x4_1(m_10).z;
          tmpvar_13.z = conv_mxt4x4_2(m_10).z;
          tmpvar_13.w = conv_mxt4x4_3(m_10).z;
          float3x3 tmpvar_14;
          tmpvar_14[0] = tmpvar_11.xyz;
          tmpvar_14[1] = tmpvar_12.xyz;
          tmpvar_14[2] = tmpvar_13.xyz;
          float3 tmpvar_15;
          tmpvar_15 = normalize(mul(tmpvar_14, in_v.normal));
          eyeNormal_7 = tmpvar_15;
          float3 tmpvar_16;
          tmpvar_16 = normalize(mul(mul(unity_MatrixV, unity_ObjectToWorld), tmpvar_9).xyz);
          viewDir_6 = (-tmpvar_16);
          lcolor_5 = (_Emission.xyz + (_Color.xyz * glstate_lightmodel_ambient.xyz));
          specColor_4 = float3(0, 0, 0);
          shininess_3 = (_Shininess * 128);
          int il_2 = 0;
          while((il_2<8))
          {
              float3 tmpvar_17;
              tmpvar_17 = unity_LightPosition[il_2].xyz;
              float3 dirToLight_18;
              dirToLight_18 = tmpvar_17;
              float3 specColor_19;
              specColor_19 = specColor_4;
              float tmpvar_20;
              tmpvar_20 = max(dot(eyeNormal_7, dirToLight_18), 0);
              float3 tmpvar_21;
              tmpvar_21 = ((tmpvar_20 * _Color.xyz) * unity_LightColor[il_2].xyz);
              if((tmpvar_20>0))
              {
                  specColor_19 = (specColor_4 + ((0.5 * clamp(pow(max(dot(eyeNormal_7, normalize((dirToLight_18 + viewDir_6))), 0), shininess_3), 0, 1)) * unity_LightColor[il_2].xyz));
              }
              specColor_4 = specColor_19;
              lcolor_5 = (lcolor_5 + min((tmpvar_21 * 0.5), float3(1, 1, 1)));
              il_2 = (il_2 + 1);
          }
          color_8.xyz = float3(lcolor_5);
          color_8.w = _Color.w;
          specColor_4 = (specColor_4 * _SpecColor.xyz);
          float4 tmpvar_22;
          float4 tmpvar_23;
          tmpvar_23 = clamp(color_8, 0, 1);
          tmpvar_22 = tmpvar_23;
          float3 tmpvar_24;
          float3 tmpvar_25;
          tmpvar_25 = clamp(specColor_4, 0, 1);
          tmpvar_24 = tmpvar_25;
          float4 tmpvar_26;
          tmpvar_26.w = 1;
          tmpvar_26.xyz = float3(tmpvar_1);
          out_v.xlv_COLOR0 = tmpvar_22;
          out_v.xlv_COLOR1 = tmpvar_24;
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_26));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 col_1;
          float4 tmpvar_2;
          tmpvar_2 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          col_1.xyz = (tmpvar_2 * in_f.xlv_COLOR0).xyz;
          col_1.xyz = (col_1 * 2).xyz;
          col_1.w = (tmpvar_2.w * in_f.xlv_COLOR0.w);
          if((col_1.w<=0))
          {
              discard;
          }
          col_1.xyz = (col_1.xyz + in_f.xlv_COLOR1);
          out_f.color = col_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
