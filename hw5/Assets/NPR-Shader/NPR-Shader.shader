Shader "Custom/NPR-Shader"
{
    Properties
    {
        [Header(Main Map)]
        _MainTex("Albedo", 2D) = "white" {}

        [Header(Warped Diffuce)]
        _WarpedTex("Warped Texture", 2D) = "white" {}
        _WarpedScale("Warped Scale", Float) = 1
        _CubeMap("Ambient Cube",Cube) = ""{}
        [Header(Specular)]
        _SpecularMask("Specular Mask", 2D) = "white" {}
        _Fspec("Fresnel Specular Term", Float) = 1
        _Kspec("Specular Exponent Power", Float) = 1

        [Header(Rim)]
        _RimMask("Rim Mask", 2D) = "white" {}
        _RimPower("Rim Power", Float) = 4
        _Krim("Rim Exponent Power", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase"}
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
            half4 _MainTex_ST;

            sampler2D _WarpedTex;
            half4 _WarpedTex_ST;
            half _WarpedScale;
            samplerCUBE _CubeMap;

            sampler2D _SpecularMask;
            half4 _SpecularMask_ST;
            half _Fspec;
            half _Kspec;

            sampler2D _RimMask;
            half4 _RimMask_ST;
            half _RimPower;
            half _Krim;


            struct a2v {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                half3 normal : NORMAL;
                half3 NdotL : TEXCOORD1;
                half3 VdotR : TEXCOORD2;
                half3 VdotN : TEXCOORD3;
                half3 NdotU : TEXCOORD4;
            };

            //ambient cube
            float3 AmbientLight(const float3 worldNormal)
            {
                float3 cAmbientCube[6];
                cAmbientCube[0] = float3(UNITY_LIGHTMODEL_AMBIENT.x, 0, 0);
                cAmbientCube[1] = float3( 0, UNITY_LIGHTMODEL_AMBIENT.y, 0);
                cAmbientCube[2] = float3( 0, 0, UNITY_LIGHTMODEL_AMBIENT.z);
                cAmbientCube[3] = float3(0.5,0.5,0.5);
                cAmbientCube[4] = float3(0.5, 0.5, 0.5);
                cAmbientCube[5] = float3(0.5, 0.5, 0.5);
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
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                half3 worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                half3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                o.normal = worldNormal;
                o.NdotL = dot(worldNormal, lightDir);

                half3 viewDir = normalize(WorldSpaceViewDir(v.vertex));
                half3 reflectDir = reflect(-lightDir, worldNormal);
                o.VdotR = saturate(dot(viewDir, reflectDir));
                o.VdotN = saturate(dot(viewDir, worldNormal));

                half3 worldUp = half3(0, 1, 0);
                o.NdotU = dot(worldNormal, worldUp);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //View Independent Lighting
                //albedo
                fixed4 albedo = tex2D(_MainTex, i.uv);
                //return albedo;

                //Warped diffuse
                half halfLambert = pow(0.5 * i.NdotL + 0.5, 2);
                half2 warpedUV = float2(halfLambert, halfLambert);
                half3 diffuseWarping = tex2D(_WarpedTex, warpedUV).rgb * _WarpedScale;
                //return fixed4(diffuseWarping,1);

                //ambient cube
                float3 ambient_cube = texCUBE(_CubeMap, i.normal).rgb;
                //return fixed4(ambient, 1.0);

                //View Dependent Lighting
                //Multiple Phong Terms
                half4 ks = tex2D(_SpecularMask, i.uv);
                half3 specularTerm = _Fspec * pow(i.VdotR, _Kspec);
                //return fixed4(_LightColor0.rgb* ks* specularTerm,1);

                //rimlighting
                half fresnelRim = pow(1 - i.VdotN, _RimPower);
                half4 kr = tex2D(_RimMask, i.uv);
                half3 rimTerm = fresnelRim * kr * pow(i.VdotN, _Krim);

                half3 multiplePhongTerms = rimTerm;
                half3 dedicatedRimLighting = i.NdotU * fresnelRim * kr;

                half4 col;
                col.rgb = multiplePhongTerms + dedicatedRimLighting;
                col.a = 1;

                return col;

                fixed4 finalColor;
                finalColor.rgb = diffuseWarping * ambient_cube;

                return finalColor;
                
              
               
            }
            ENDCG
        }
    }
}
