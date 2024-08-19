using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SnakeTail : NetworkBehaviour
{
    public Transform SnakeTailGfx;
    public float circleDiameter;
    private List<Transform> snakeTail = new List<Transform>();
    private List<Vector2> positions = new List<Vector2>();

    private void Start()
    {
        if (isServer)  // Only initialize on the server
        {
            positions.Add(SnakeTailGfx.position);
        }
    }

    private void Update()
    {
        if (!isServer)
        {
            return; // Only update the tail on the server
        }

        float distance = ((Vector2)SnakeTailGfx.position - positions[0]).magnitude;

        if (distance > circleDiameter)
        {
            Vector2 direction = ((Vector2)SnakeTailGfx.position - positions[0]).normalized;

            positions.Insert(0, positions[0] + direction * circleDiameter);
            positions.RemoveAt(positions.Count - 1);
            distance -= circleDiameter;
        }

        for (int i = 0; i < snakeTail.Count; i++)
        {
            snakeTail[i].position = Vector2.Lerp(positions[i + 1], positions[i], distance / circleDiameter);
        }
    }

    [Command]
    public void CmdAddTail()
    {
        AddTail();
        RpcAddTail();  // Notify all clients about the new tail segment
    }

    [ClientRpc]
    void RpcAddTail()
    {
        // This ensures that all clients update their tail segments
        if (isServer) return;  // Avoid creating another tail on the server

        // Create the tail segment locally
        AddTail();
    }

    public void AddTail()
    {
        // Instantiate a new tail segment
        Transform tail = Instantiate(SnakeTailGfx, positions[positions.Count - 1], Quaternion.identity, transform);
        NetworkServer.Spawn(tail.gameObject);  // Spawn it on the network
        snakeTail.Add(tail);
        positions.Add(tail.position);
    }
}
