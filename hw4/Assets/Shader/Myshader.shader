Shader "Custom/Myshader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Shininess("Shininess",range(1,32)) = 8
        //_MainColor("Main Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Pass 
        {
            CGPROGRAM

            #include "UnityStandardBRDF.cginc"
            #include "UnityCG.cginc"
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram
           
            sampler2D _MainTex; 
            float4 _MainTex_ST;
            float _Shininess;

           /* struct VertexData {
                float4 position : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct FragmentData {
                float4 position : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };*/
            
            struct VertexData {
                float4 position : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
            struct FragmentData {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            FragmentData MyVertexProgram(VertexData v) {
                FragmentData i;
                i.position = UnityObjectToClipPos(v.position);
                i.normal = UnityObjectToWorldNormal(v.normal);
                i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                i.worldPos = mul(unity_ObjectToWorld, v.position);
                return i;
            }

            
            float4 MyFragmentProgram(FragmentData i) : SV_TARGET{
               
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float3 lightColor = _LightColor0.rgb;

                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 halfVector = normalize(lightDir + viewDir);

                //diffuse
                float3 diffuse = tex2D(_MainTex, i.uv).rgb * lightColor * DotClamped(lightDir, i.normal);

                //specular
                float spec = pow(max(dot(i.normal, halfVector), 0.0), _Shininess);
                float3 specular = lightColor * spec;
                return float4(diffuse+specular, 1);
            }
            
            ENDCG
        }
    }
}
