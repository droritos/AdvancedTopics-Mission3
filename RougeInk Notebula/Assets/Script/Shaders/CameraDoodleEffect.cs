using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraDoodleEffect : MonoBehaviour
{
    private Material doodleMaterial;
    
    [Header("Juice Settings")]
    [Tooltip("How many times per second the wobble should update (e.g. 12 FPS for animation style)")]
    [Range(1f, 24f)] public float wobbleSpeed = 8f;
    
    [Tooltip("How intense the distortion is")]
    [Range(0.0001f, 0.01f)] public float wobbleStrength = 0.002f;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (doodleMaterial == null)
        {
            Shader shader = Shader.Find("Hidden/DoodleWobble");
            if (shader == null) 
            {
                Graphics.Blit(source, destination);
                return;
            }
            doodleMaterial = new Material(shader);
        }
        
        doodleMaterial.SetFloat("_WobbleSpeed", wobbleSpeed);
        doodleMaterial.SetFloat("_WobbleStrength", wobbleStrength);

        Graphics.Blit(source, destination, doodleMaterial);
    }

    private void OnDisable()
    {
        if (doodleMaterial != null)
        {
            DestroyImmediate(doodleMaterial);
        }
    }
}
