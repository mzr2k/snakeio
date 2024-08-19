using Mirror;
using UnityEngine;

public class SnakeMove : NetworkBehaviour
{
    public float speed = 3f;
    public float rotationSpeed = 200f;

    private float velX = 0f;
    private float velY = 0f;

    void Update()
    {
        

        if (!isLocalPlayer)
        {
            return; 
        }

        // Get input
        velX = Input.GetAxisRaw("Horizontal");
        velY = Input.GetAxisRaw("Vertical");

        // Movement
        Vector2 movement = new Vector2(velX, velY);
        transform.Translate(movement * speed * Time.deltaTime);

        // Rotation
        transform.Rotate(Vector3.forward * -velX * rotationSpeed * Time.fixedDeltaTime);
    }
}
