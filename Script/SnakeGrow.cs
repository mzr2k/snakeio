using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SnakeGrow : NetworkBehaviour
{
    public SnakeTail snakeTail;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isServer) return; // Ensure that only the server handles the growth

        if (other.gameObject.CompareTag("Food"))
        {
            Debug.Log("eat");
            Destroy(other.gameObject, 0.02f); // Destroy the food object after a short delay
            GrowSnake(); // Call the server-side method to grow the snake
        }
    }

    [Server]
    void GrowSnake()
    {
        snakeTail.AddTail(); // Add a tail segment
        RpcUpdateGrowth(); // Notify all clients to update their tail segments
    }

    [ClientRpc]
    void RpcUpdateGrowth()
    {
        // This ensures that all clients see the new tail segment
        // If you want to handle additional logic per client, you can add it here
    }
}
