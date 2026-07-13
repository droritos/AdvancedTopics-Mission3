using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CreatePaperOverlay : EditorWindow
{
    [MenuItem("GameObject/Juice/Generate Paper Overlay Prefab", false, 10)]
    public static void CreateOverlay()
    {
        // Create the root canvas
        GameObject canvasGO = new GameObject("PaperOverlayCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100; // Render on top of everything
        
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Create the paper overlay image object
        GameObject overlayGO = new GameObject("PaperOverlay");
        overlayGO.transform.SetParent(canvasGO.transform, false);

        // Setup the image for paper texture
        Image img = overlayGO.AddComponent<Image>();
        
        // Try to load the newly generated paper texture!
        Sprite paperSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/notebook_paper_texture.png");
        if (paperSprite != null)
        {
            img.sprite = paperSprite;
            img.color = new Color(1f, 1f, 1f, 0.35f); // Slightly transparent to let the game show through
        }
        else
        {
            img.color = new Color(0.95f, 0.92f, 0.88f, 0.15f); // Fallback color
        }
        
        img.raycastTarget = false;

        // Stretch to fill
        RectTransform rt = overlayGO.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        // Attach controller
        overlayGO.AddComponent<PaperOverlayController>();

        // Save as Prefab
        if (!System.IO.Directory.Exists("Assets/Prefab"))
        {
            System.IO.Directory.CreateDirectory("Assets/Prefab");
        }
        
        string localPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Prefab/PaperOverlayCanvas.prefab");
        PrefabUtility.SaveAsPrefabAssetAndConnect(canvasGO, localPath, InteractionMode.UserAction);
        
        Debug.Log("Paper Overlay Prefab successfully generated at: " + localPath);
    }
}
