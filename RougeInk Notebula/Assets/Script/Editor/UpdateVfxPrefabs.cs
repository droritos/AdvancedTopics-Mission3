using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class UpdateVfxPrefabs
{
    static UpdateVfxPrefabs()
    {
        EditorApplication.delayCall += UpdatePrefabs;
    }

    static void UpdatePrefabs()
    {
        bool modified = false;

        GameObject collisionVFX = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefab/VFX/CollisionVFX.prefab");
        if (collisionVFX != null)
        {
            if (collisionVFX.GetComponent<ReturnVfxToPool>() == null)
            {
                var returnScript = collisionVFX.AddComponent<ReturnVfxToPool>();
                returnScript.delay = 1f;
                EditorUtility.SetDirty(collisionVFX);
                modified = true;
            }
        }

        GameObject inkSplashVFX = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefab/VFX/InkSplash.prefab");
        if (inkSplashVFX != null)
        {
            if (inkSplashVFX.GetComponent<ReturnVfxToPool>() == null)
            {
                var returnScript = inkSplashVFX.AddComponent<ReturnVfxToPool>();
                returnScript.delay = 2f;
                EditorUtility.SetDirty(inkSplashVFX);
                modified = true;
            }
        }

        if (modified)
        {
            AssetDatabase.SaveAssets();
            Debug.Log("[AI] Attached ReturnVfxToPool scripts to VFX prefabs!");
        }
    }
}
