using UnityEngine;
using System.Collections;

public class SpawnBackground : MonoBehaviour
{
    [Header("Game Object")]
    [SerializeField] GameObject planetsAndStarsBackground;
    private Transform _mainCameraTransform;
    private Transform _particalParents;

    [Header("Private Data")]
    [SerializeField] float spawnInterval = 10f; // Interval in seconds
    private GameObject _currentBackground;

    private void Awake()
    {
        _mainCameraTransform = GameObject.Find("MainCamera").transform;
        _particalParents = GameObject.Find("ParticalParents").transform;
        _currentBackground = Instantiate(planetsAndStarsBackground, Vector3.zero, Quaternion.identity, _particalParents);
        StartCoroutine(SpawnParticalCoroutine());
    }

    private IEnumerator SpawnParticalCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (_currentBackground != null)
            {
                Destroy(_currentBackground);
            }
            _currentBackground = Instantiate(planetsAndStarsBackground, _mainCameraTransform.position, Quaternion.identity, _particalParents);
        }
    }
}
