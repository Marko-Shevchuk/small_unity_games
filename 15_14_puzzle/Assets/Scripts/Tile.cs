using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isEmpty;

    public bool IsEmpty()
    {
        return isEmpty;
    }
    public bool CanMoveTo(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        return distance<1.1;
    }

    public void SwapWith(Tile other)
    {
        Vector3 tempPosition = transform.position;
        transform.position = other.transform.position;
        other.transform.position = tempPosition;
    }

    public Tile GetRandomNeighbourTile(Tile[,] tiles)
    {
        List<Tile> neighbours = new List<Tile>();
        int x = (int)(transform.position.x + 0.5) - 1;
        int z = (int)(transform.position.z + 0.5) - 1;

        
        if (x > 0 && tiles[x - 1, z] != null && tiles[x - 1, z].CanMoveTo(transform.position)) neighbours.Add(tiles[x - 1, z]); // Left
                                                                                                                               
        if (x < tiles.GetLength(0) - 1 && tiles[x + 1, z] != null && tiles[x + 1, z].CanMoveTo(transform.position)) neighbours.Add(tiles[x + 1, z]); // Right
                                                                                                                                               
        if (z > 0 && tiles[x, z - 1] != null && tiles[x, z - 1].CanMoveTo(transform.position)) neighbours.Add(tiles[x, z - 1]); // Up
                                                                                                                       
        if (z < tiles.GetLength(1) -1 && tiles[x, z + 1] != null && tiles[x, z + 1].CanMoveTo(transform.position)) neighbours.Add(tiles[x, z + 1]); // Down

        if (neighbours.Count > 0)
        {
            return neighbours[Random.Range(0, neighbours.Count)];
        }
        else
        {
            Debug.LogError("No valid neighbour for tile at position: " + transform.position);
            return null; 
        }
    }
}