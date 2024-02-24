using System.Collections.Generic;
using UnityEngine;
public class Tile : MonoBehaviour
{
    public bool isEmpty;
    private AudioSource tap;


    void Start()
    {
        tap = GetComponent<AudioSource>();
    }
    public void playTap()
    {

        if (tap != null)
        {
            tap.Stop();
            tap.Play();
        }

    }
    public bool IsEmpty()
    {
        return isEmpty;
    }
    public bool CanMoveTo(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        return distance<1.1 && distance > 0;
    }



    public Tile GetRandomNeighbourTile(Tile[,] tiles)
    {
        List<Tile> neighbours = new List<Tile>();
        int x = (int)(transform.position.x + 0.5) - 1;
        int z = (int)(transform.position.z + 0.5) - 1;

        for (int i = 0; i< tiles.Length; i++)
        {
            if ( (tiles[i / 4, i % 4] != null) && tiles[i / 4, i % 4].CanMoveTo(transform.position)) neighbours.Add(tiles[i / 4, i % 4]);
        }
  
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