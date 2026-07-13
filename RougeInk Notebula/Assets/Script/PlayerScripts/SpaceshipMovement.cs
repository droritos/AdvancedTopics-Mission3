using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipMovement : MonoBehaviour
{   
    [SerializeField] float shipSpeed = 10f;
    [HideInInspector] public bool isMove = false;
    public bool isAlive = true;

    void Update()
    {
        ShipMovement();
    }

    void ShipMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Note: Getting acces to the Horizontal values 
        float verticalInput = Input.GetAxis("Vertical"); // Note: Getting acces to the Vertical values
        transform.Translate(new Vector2(horizontalInput * Time.deltaTime * shipSpeed, verticalInput * Time.deltaTime * shipSpeed));
        // Note: Time.deltaTime fixing the frames between diffrents PC or Consoles 
        if (horizontalInput != 0 || verticalInput != 0) // 0 define the input of the player
            isMove = true;
        else
            isMove = false;
    }
}
