Shader "Custom/shader5"
{
    Properties
    {
        _Color("Color Tint", Color) = (1, 1, 1, 1)
        _MainTex("Main Tex", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Bump Scale", Float) = 1.0
        _Specular("Specular", Color) = (1, 1, 1, 1)
        _Gloss("Gloss", Range(8.0, 256)) = 20
    }

    SubShader
    {

        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass 
        {
            Tags { "LightMode" = "ForwardBase" }
            CGPROGRAM

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature USE_SPECULAR
            #pragma shader_feature RENDERING_MODE_BLINN
            #pragma shader_feature RENDERING_MODE_NORMAL
           
            fixed4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BumpMap;
            float4 _BumpMap_ST;
            float _BumpScale;
            fixed4 _Specular;
            float _Gloss;

            struct a2v {
                float4 position : POSITION;
                fixed3 normal : NORMAL;
                fixed4 texcoord : TEXCOORD0;
                float4 tangent : TANGENT;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float3 lightDir  : TEXCOORD1;
                float3 viewDir  : TEXCOORD2;
                float3 normal : TEXCOORD3;
                float3 worldPos : TEXCOORD4;
            };

            v2f vert(a2v v) {
                #if RENDERING_MODE_BLINN
                v2f i;
                i.pos = UnityObjectToClipPos(v.position);
                i.normal = UnityObjectToWorldNormal(v.normal);
                i.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                i.worldPos = mul(unity_ObjectToWorld, v.position);
                return i;
                #endif
                v2f o;
                o.pos = UnityObjectToClipPos(v.position);

                o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
                
                //Create a rotation matrix for tangent space
                TANGENT_SPACE_ROTATION;
                //Transform the light direction from object space to tangent space
                o.lightDir = mul(rotation, ObjSpaceLightDir(v.position)).xyz;
                //Transform the view direction from object space to tangent space
                o.viewDir = mul(rotation, ObjSpaceViewDir(v.position)).xyz;
              
                return o;
            }

            
            fixed4 frag(v2f i) : SV_TARGET{
                //BLINN PHONG MODE
                #if RENDERING_MODE_BLINN
                float3 lightDir_b = _WorldSpaceLightPos0.xyz;
                float3 lightColor_b = _LightColor0.rgb;

                float3 viewDir_b = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 halfVector = normalize(lightDir_b + viewDir_b);

                //ambient
                fixed3 ambient_b = UNITY_LIGHTMODEL_AMBIENT.xyz * tex2D(_MainTex, i.uv).rgb;

                //diffuse
                float3 diffuse_b= tex2D(_MainTex, i.uv).rgb * lightColor_b * DotClamped(lightDir_b, i.normal);
                #if RENDERING_MODE_NORMAL
                diffuse_b = tex2D(_MainTex, i.uv).rgb * lightColor_b * DotClamped(lightDir_b, i.normal);
                return float4(ambient_b + diffuse_b, 1);
                #endif

                //specular
                float3 specular_b = float3(0, 0, 0);
                #if USE_SPECULAR
                float spec_b = pow(max(dot(i.normal, halfVector), 0.0), _Gloss);
                specular_b = lightColor_b * spec_b;
                #endif
                return float4(ambient_b + diffuse_b + specular_b, 1);
                #endif

                //NORMAL TEXTURE
                fixed3 tangentLightDir = normalize(i.lightDir);
                fixed3 tangentViewDir = normalize(i.viewDir);

                fixed4 texColor = tex2D(_MainTex, i.uv)* _Color;
                fixed4 packedNormal = tex2D(_BumpMap, i.uv.zw);
                fixed3 tnormal = UnpackNormal(packedNormal);
   
                //fixed3 tnormal;
                tnormal.xy *= _BumpScale;
                tnormal.z = sqrt(1.0 - saturate(dot(tnormal.xy, tnormal.xy)));

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

                fixed3 diffuse = _LightColor0.rgb * texColor * saturate(dot(tnormal, tangentLightDir));

                fixed3 halfDir = normalize(tangentLightDir + tangentViewDir);

                //specular
                fixed3 specular = float3(0, 0, 0);
                #if USE_SPECULAR
                specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(tnormal, halfDir)), _Gloss);
                #endif

                return fixed4(ambient + diffuse + specular, 1.0);
            }
            
            ENDCG
        }
    }
            CustomEditor "CustomShaderGUI"
}
