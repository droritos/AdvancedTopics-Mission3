Shader "Custom/SpriteFlash"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _FlashColor ("Flash Color", Color) = (1,1,1,1)
        _FlashAmount ("Flash Amount", Range(0,1)) = 0
        _WobbleSpeed ("Wobble Speed (FPS)", Float) = 12
        _WobbleAmount ("Wobble Amount", Float) = 0.05
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
            };
            
            fixed4 _Color;
            float _WobbleSpeed;
            float _WobbleAmount;

            // Simple pseudo-random function
            float hash(float2 p) {
                return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
            }

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                
                // Quantize time to create discrete "frames" of animation (Boiling Line effect)
                float discreteTime = floor(_Time.y * _WobbleSpeed);
                
                // Add pseudo-random offset based on the quantized time and the vertex's local position
                float noiseX = hash(IN.vertex.xy + float2(discreteTime, 0.0)) * 2.0 - 1.0;
                float noiseY = hash(IN.vertex.xy + float2(0.0, discreteTime)) * 2.0 - 1.0;
                
                float4 wobbledVertex = IN.vertex;
                wobbledVertex.x += noiseX * _WobbleAmount;
                wobbledVertex.y += noiseY * _WobbleAmount;
                
                OUT.vertex = UnityObjectToClipPos(wobbledVertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float _AlphaSplitEnabled;
            fixed4 _FlashColor;
            float _FlashAmount;

            fixed4 SampleSpriteTexture (float2 uv)
            {
                fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
                if (_AlphaSplitEnabled)
                    color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
                
                // Purely visual flash using the lerp function!
                c.rgb = lerp(c.rgb, _FlashColor.rgb * c.a, _FlashAmount);
                
                c.rgb *= c.a;
                return c;
            }
        ENDCG
        }
    }
}
