using System.Collections.Generic;
using UnityEngine;

public class BackgorundManager : MonoBehaviour
{
    public static BackgorundManager Instance; // The "Singleton Pattern" - Easy way to call the BackgorundManager script from everywhere
    public List<GameObject> backgrounds;
    [SerializeField] GameObject backgroundPrefab;
    private Transform mainCameraTransform;
    [SerializeField] float maxDistance;
    Transform backgroundsParent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        mainCameraTransform = GameObject.Find("MainCamera")?.transform;
        backgroundsParent = GameObject.Find("BackgroundParents")?.transform;
    }

    private void CheckDistanceToDestroy(GameObject background)
    {
        if(background != null && mainCameraTransform != null)
        {
            Vector3 difference = mainCameraTransform.position - background.transform.position;
            float distanceFromCamera = difference.magnitude;
            if (distanceFromCamera >= maxDistance)
            {
                backgrounds.Remove(background);
                Destroy(background);
            }
        }
    }

    public void CheckAllBackgrounds()
    {
        for (int i = backgrounds.Count - 1; i >= 0; i--) // Iterate backwards to safely remove items from the list
        {
            if (backgrounds[i] != null)
            {
                CheckDistanceToDestroy(backgrounds[i]);
            }
        }
    }

    public void SpawnBackground(Vector3 positionToSpawnIn)
    {
        bool isBackgroundInPosition = false;
        for (int i = 0; i < backgrounds.Count; i++)
        {
            if (backgrounds[i] != null && backgrounds[i].transform.position == positionToSpawnIn)
            {
                isBackgroundInPosition = true;
                break; // Exit loop early if background is found
            }
        }

        if (!isBackgroundInPosition && backgroundsParent != null)
        {
            GameObject newBackground = Instantiate(backgroundPrefab, positionToSpawnIn, Quaternion.identity, backgroundsParent);
            backgrounds.Add(newBackground);
        }
    }
}
