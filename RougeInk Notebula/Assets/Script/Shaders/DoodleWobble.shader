Shader "Hidden/DoodleWobble"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WobbleSpeed ("Wobble Speed (FPS)", Float) = 12.0
        _WobbleStrength ("Wobble Strength", Float) = 0.002
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _WobbleSpeed;
            float _WobbleStrength;

            // Simple pseudo-random function
            float random(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Discretize time to create a stop-motion/hand-drawn 12fps animation feel
                float steppedTime = floor(_Time.y * _WobbleSpeed);
                
                // Calculate pseudo-random offset for x and y
                float offsetX = (random(i.uv + steppedTime) - 0.5) * 2.0;
                float offsetY = (random(i.uv - steppedTime) - 0.5) * 2.0;
                
                float2 uvOffset = float2(offsetX, offsetY) * _WobbleStrength;
                
                // Apply the wobble to the final render output!
                fixed4 col = tex2D(_MainTex, i.uv + uvOffset);
                return col;
            }
            ENDCG
        }
    }
}
