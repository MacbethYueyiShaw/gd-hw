Shader "Custom/NPR-Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Bump Scale", Float) = 1.0
        _Specular("Specular", Color) = (1, 1, 1, 1)
        _Gloss("Gloss", Range(8.0, 256)) = 20
        _Color("Color Tint", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BumpMap;
            float4 _BumpMap_ST;
            float _BumpScale;
            fixed4 _Specular;
            float _Gloss;
            fixed4 _Color;


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

            //ambient cube
            float3 AmbientLight(const float3 worldNormal)
            {
                float3 cAmbientCube[6];
                cAmbientCube[0] = float3(UNITY_LIGHTMODEL_AMBIENT.x,0,0);
                cAmbientCube[1] = float3( 0, UNITY_LIGHTMODEL_AMBIENT.y, 0);
                cAmbientCube[2] = float3( 0, 0, UNITY_LIGHTMODEL_AMBIENT.z);
                cAmbientCube[3] = float3(1,1,1);
                cAmbientCube[4] = float3(1,1,1);
                cAmbientCube[5] = float3(1,1,1);
                float3 nSquared = worldNormal * worldNormal;
                int3 isNegative = (worldNormal < 0.0);
                float3 linearColor;
                linearColor = nSquared.x * cAmbientCube[isNegative.x] +
                    nSquared.y * cAmbientCube[isNegative.y + 2] +
                    nSquared.z * cAmbientCube[isNegative.z + 4];
                return linearColor;
            }

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

            fixed4 frag (v2f i) : SV_Target
            {
                
                fixed3 tangentLightDir = normalize(i.lightDir);
                fixed3 tangentViewDir = normalize(i.viewDir);

                //albedo
                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;
                //return texColor;

                fixed4 packedNormal = tex2D(_BumpMap, i.uv.zw);
                fixed3 tnormal = UnpackNormal(packedNormal);

                //fixed3 tnormal;
                tnormal.xy *= _BumpScale;
                tnormal.z = sqrt(1.0 - saturate(dot(tnormal.xy, tnormal.xy)));
                
                //ambient
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
                //fixed3 ambient = AmbientLight(tnormal);
                return fixed4(ambient, 1.0);

                fixed3 diffuse = _LightColor0.rgb * texColor * saturate(dot(tnormal, tangentLightDir));

                fixed3 halfDir = normalize(tangentLightDir + tangentViewDir);

                //specular
                fixed3 specular = float3(0, 0, 0);
                specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(tnormal, halfDir)), _Gloss);

                return fixed4(ambient + diffuse + specular, 1.0);
            }
            ENDCG
        }
    }
}
