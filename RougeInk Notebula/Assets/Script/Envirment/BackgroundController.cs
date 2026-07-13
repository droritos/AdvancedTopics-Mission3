using System.Collections.Specialized;
using UnityEditor;
using UnityEngine;
public class BackgroundController : MonoBehaviour
{
    Transform mainCameraTransform;
    private void Start()
    {
        mainCameraTransform = GameObject.Find("MainCamera").transform;
        BackgorundManager.Instance.backgrounds.Add(this.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collisionObject)
    {
        BackgorundManager.Instance.CheckAllBackgrounds();
        if (collisionObject.CompareTag("Player") && gameObject != null && mainCameraTransform != null)
        {
            if (mainCameraTransform.position.y > gameObject.transform.position.y)
            {
                OnExitPositiveYDirection();
            }
            else if (mainCameraTransform.position.y < gameObject.transform.position.y)
            {
                OnExitNegativeYDirection();
            }
            if (mainCameraTransform.position.x > gameObject.transform.position.x)
            {
                OnExitPositiveXDirection();
            }
            else if (mainCameraTransform.position.x < gameObject.transform.position.x)
            {
                OnExitNegativeXDirection();
            }
        }
        BackgorundManager.Instance.CheckAllBackgrounds();
    }

    private void OnExitNegativeXDirection()
    {
        Vector3 topVector3 = new Vector3(transform.position.x - (79 * 2), transform.position.y + 81.5f, transform.position.z);
        Vector3 middleVector3 = new Vector3(transform.position.x - (79 * 2), transform.position.y, transform.position.z);
        Vector3 botVector3 = new Vector3(transform.position.x - (79 * 2), transform.position.y - 81.5f, transform.position.z);
        BackgorundManager.Instance.SpawnBackground(topVector3);
        BackgorundManager.Instance.SpawnBackground(middleVector3);
        BackgorundManager.Instance.SpawnBackground(botVector3);
    }
    private void OnExitPositiveXDirection()
    {
        Vector3 topVector3 = new Vector3(transform.position.x + (79 * 2), transform.position.y + 81.5f, transform.position.z);
        Vector3 middleVector3 = new Vector3(transform.position.x + (79 * 2), transform.position.y, transform.position.z);
        Vector3 botVector3 = new Vector3(transform.position.x + (79 * 2), transform.position.y - 81.5f, transform.position.z);
        BackgorundManager.Instance.SpawnBackground(topVector3);
        BackgorundManager.Instance.SpawnBackground(middleVector3);
        BackgorundManager.Instance.SpawnBackground(botVector3);
    }

    private void OnExitPositiveYDirection()
    {
        Vector3 topVector3 = new Vector3(transform.position.x, transform.position.y + (81.5f * 2), transform.position.z);
        Vector3 rightVector3 = new Vector3(transform.position.x + 79, transform.position.y + (81.5f * 2), transform.position.z);
        Vector3 leftVector3 = new Vector3(transform.position.x - 79, transform.position.y + (81.5f * 2), transform.position.z);
        BackgorundManager.Instance.SpawnBackground(topVector3);
        BackgorundManager.Instance.SpawnBackground(rightVector3);
        BackgorundManager.Instance.SpawnBackground(leftVector3);

    }

    private void OnExitNegativeYDirection()
    {
        Vector3 topVector3 = new Vector3(transform.position.x, transform.position.y - (81.5f * 2), transform.position.z);
        Vector3 rightVector3 = new Vector3(transform.position.x + 79, transform.position.y - (81.5f * 2), transform.position.z);
        Vector3 leftVector3 = new Vector3(transform.position.x - 79, transform.position.y - (81.5f * 2), transform.position.z);
        BackgorundManager.Instance.SpawnBackground(topVector3);
        BackgorundManager.Instance.SpawnBackground(rightVector3);
        BackgorundManager.Instance.SpawnBackground(leftVector3);
    }
}
