// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable

Shader "Transparent/Diffuse"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
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
    LOD 200
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 200
      ZClip Off
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
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
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
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
          float3x3 tmpvar_2;
          tmpvar_2[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_2[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_2[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_3;
          tmpvar_3 = normalize(mul(in_v.normal, tmpvar_2));
          float3 normal_4;
          normal_4 = tmpvar_3;
          float4 tmpvar_5;
          tmpvar_5.w = 1;
          tmpvar_5.xyz = float3(normal_4);
          float3 res_6;
          float3 x_7;
          x_7.x = dot(unity_SHAr, tmpvar_5);
          x_7.y = dot(unity_SHAg, tmpvar_5);
          x_7.z = dot(unity_SHAb, tmpvar_5);
          float3 x1_8;
          float4 tmpvar_9;
          tmpvar_9 = (normal_4.xyzz * normal_4.yzzx);
          x1_8.x = dot(unity_SHBr, tmpvar_9);
          x1_8.y = dot(unity_SHBg, tmpvar_9);
          x1_8.z = dot(unity_SHBb, tmpvar_9);
          res_6 = (x_7 + (x1_8 + (unity_SHC.xyz * ((normal_4.x * normal_4.x) - (normal_4.y * normal_4.y)))));
          float3 tmpvar_10;
          float _tmp_dvx_0 = max(((1.055 * pow(max(res_6, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_10 = float3(_tmp_dvx_0, _tmp_dvx_0, _tmp_dvx_0);
          res_6 = tmpvar_10;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_1));
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_3;
          out_v.xlv_TEXCOORD2 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          out_v.xlv_TEXCOORD3 = max(float3(0, 0, 0), tmpvar_10);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 tmpvar_1;
          float3 tmpvar_2;
          float3 tmpvar_3;
          float3 lightDir_4;
          float3 tmpvar_5;
          tmpvar_5 = _WorldSpaceLightPos0.xyz;
          lightDir_4 = tmpvar_5;
          tmpvar_3 = in_f.xlv_TEXCOORD1;
          float3 tmpvar_6;
          float tmpvar_7;
          float4 c_8;
          float4 tmpvar_9;
          tmpvar_9 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          float4 tmpvar_10;
          tmpvar_10 = (tmpvar_9 * _Color);
          c_8 = tmpvar_10;
          tmpvar_6 = c_8.xyz;
          tmpvar_7 = c_8.w;
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_4;
          float4 c_11;
          float4 c_12;
          float diff_13;
          float tmpvar_14;
          tmpvar_14 = max(0, dot(tmpvar_3, tmpvar_2));
          diff_13 = tmpvar_14;
          c_12.xyz = float3(((tmpvar_6 * tmpvar_1) * diff_13));
          c_12.w = tmpvar_7;
          c_11.w = c_12.w;
          c_11.xyz = (c_12.xyz + (tmpvar_6 * in_f.xlv_TEXCOORD3));
          out_f.color = c_11;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "FORWARDADD"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 200
      ZClip Off
      ZWrite Off
      Blend SrcAlpha One
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile POINT
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _LightTexture0;
      uniform float4x4 unity_WorldToLight;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
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
          float3x3 tmpvar_2;
          tmpvar_2[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_2[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_2[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_1));
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = normalize(mul(in_v.normal, tmpvar_2));
          out_v.xlv_TEXCOORD2 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 tmpvar_1;
          float3 tmpvar_2;
          float3 lightCoord_3;
          float3 tmpvar_4;
          float3 lightDir_5;
          float3 tmpvar_6;
          tmpvar_6 = normalize((_WorldSpaceLightPos0.xyz - in_f.xlv_TEXCOORD2));
          lightDir_5 = tmpvar_6;
          tmpvar_4 = in_f.xlv_TEXCOORD1;
          float3 tmpvar_7;
          float tmpvar_8;
          float4 c_9;
          float4 tmpvar_10;
          tmpvar_10 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          float4 tmpvar_11;
          tmpvar_11 = (tmpvar_10 * _Color);
          c_9 = tmpvar_11;
          tmpvar_7 = c_9.xyz;
          tmpvar_8 = c_9.w;
          float4 tmpvar_12;
          tmpvar_12.w = 1;
          tmpvar_12.xyz = in_f.xlv_TEXCOORD2;
          lightCoord_3 = mul(unity_WorldToLight, tmpvar_12).xyz;
          float tmpvar_13;
          tmpvar_13 = dot(lightCoord_3, lightCoord_3);
          float tmpvar_14;
          tmpvar_14 = tex2D(_LightTexture0, float2(tmpvar_13, tmpvar_13)).w;
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_5;
          tmpvar_1 = (tmpvar_1 * tmpvar_14);
          float4 c_15;
          float4 c_16;
          float diff_17;
          float tmpvar_18;
          tmpvar_18 = max(0, dot(tmpvar_4, tmpvar_2));
          diff_17 = tmpvar_18;
          c_16.xyz = float3(((tmpvar_7 * tmpvar_1) * diff_17));
          c_16.w = tmpvar_8;
          c_15.w = c_16.w;
          c_15.xyz = c_16.xyz;
          out_f.color = c_15;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: META
    {
      Name "META"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "META"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 200
      ZClip Off
      ZWrite Off
      Cull Off
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
      // uniform float4 unity_DynamicLightmapST;
      uniform float4 unity_MetaVertexControl;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      uniform float4 unity_MetaFragmentControl;
      uniform float unity_OneOverOutputBoost;
      uniform float unity_MaxOutputValue;
      uniform float unity_UseLinearSpace;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
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
          float4 vertex_1;
          vertex_1 = in_v.vertex;
          if(unity_MetaVertexControl.x)
          {
              vertex_1.xy = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
              float tmpvar_2;
              if((in_v.vertex.z>0))
              {
                  tmpvar_2 = 0.0001;
              }
              else
              {
                  tmpvar_2 = 0;
              }
              vertex_1.z = tmpvar_2;
          }
          if(unity_MetaVertexControl.y)
          {
              vertex_1.xy = ((in_v.texcoord2.xy * unity_DynamicLightmapST.xy) + unity_DynamicLightmapST.zw);
              float tmpvar_3;
              if((vertex_1.z>0))
              {
                  tmpvar_3 = 0.0001;
              }
              else
              {
                  tmpvar_3 = 0;
              }
              vertex_1.z = tmpvar_3;
          }
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = vertex_1.xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_4));
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float3 tmpvar_2;
          float3 tmpvar_3;
          float4 c_4;
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          float4 tmpvar_6;
          tmpvar_6 = (tmpvar_5 * _Color);
          c_4 = tmpvar_6;
          tmpvar_3 = c_4.xyz;
          tmpvar_2 = tmpvar_3;
          float4 res_7;
          res_7 = float4(0, 0, 0, 0);
          if(unity_MetaFragmentControl.x)
          {
              float4 tmpvar_8;
              tmpvar_8.w = 1;
              tmpvar_8.xyz = float3(tmpvar_2);
              res_7.w = tmpvar_8.w;
              float3 tmpvar_9;
              float _tmp_dvx_1 = clamp(unity_OneOverOutputBoost, 0, 1);
              tmpvar_9 = clamp(pow(tmpvar_2, float3(_tmp_dvx_1, _tmp_dvx_1, _tmp_dvx_1)), float3(0, 0, 0), float3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue));
              res_7.xyz = float3(tmpvar_9);
          }
          if(unity_MetaFragmentControl.y)
          {
              float3 emission_10;
              if(int(unity_UseLinearSpace))
              {
                  emission_10 = float3(0, 0, 0);
              }
              else
              {
                  emission_10 = float3(0, 0, 0);
              }
              float4 tmpvar_11;
              float alpha_12;
              float3 tmpvar_13;
              tmpvar_13 = (emission_10 * 0.01030928);
              alpha_12 = (ceil((max(max(tmpvar_13.x, tmpvar_13.y), max(tmpvar_13.z, 0.02)) * 255)) / 255);
              float tmpvar_14;
              tmpvar_14 = max(alpha_12, 0.02);
              alpha_12 = tmpvar_14;
              float4 tmpvar_15;
              tmpvar_15.xyz = float3((tmpvar_13 / tmpvar_14));
              tmpvar_15.w = tmpvar_14;
              tmpvar_11 = tmpvar_15;
              res_7 = tmpvar_11;
          }
          tmpvar_1 = res_7;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Transparent/VertexLit"
}
