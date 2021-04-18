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
            };

            v2f vert(a2v v) {
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
                fixed3 tangentLightDir = normalize(i.lightDir);
                fixed3 tangentViewDir = normalize(i.viewDir);

                fixed4 texColor = tex2D(_MainTex, i.uv)* _Color;
                //fixed3 tnormal = UnpackNormal(tex2D(_BumpMap, i.uv));
                fixed4 packedNormal = tex2D(_BumpMap, i.uv.zw);
                fixed3 tnormal;
                tnormal.xy = (2 * packedNormal - 1) * _BumpScale;
                tnormal.z = sqrt(1.0 - saturate(dot(tnormal.xy, tnormal.xy)));

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

                fixed3 diffuse = _LightColor0.rgb * texColor * saturate(dot(tnormal, tangentLightDir));

                fixed3 halfDir = normalize(tangentLightDir + tangentViewDir);
                fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(tnormal, halfDir)), _Gloss);

                return fixed4(ambient + diffuse + specular, 1.0);
            }
            
            ENDCG
        }
    }
}
