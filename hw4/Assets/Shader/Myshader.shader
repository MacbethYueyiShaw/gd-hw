Shader "Custom/Myshader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
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
                float3 diffuse = tex2D(_MainTex, i.uv).rgb * lightColor * DotClamped(lightDir, i.normal);
                return float4(diffuse, 1);
            }
            
            ENDCG
        }
    }
}
