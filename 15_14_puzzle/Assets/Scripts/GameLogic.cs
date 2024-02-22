using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Tile[,] tiles = new Tile[4, 4];
    private Tile emptyTile;
    private Tile tempTile;
    void Start()
    {
        GameObject[] tileObjects = GameObject.FindGameObjectsWithTag("Tile");
        // Initialize the tiles array with the found GameObjects
        /*for (int i = 0; i < tileObjects.Length; i++)
        {
            Tile tile = tileObjects[i].GetComponent<Tile>();
            tiles[((int)tile.transform.position.x + 0.5)-1, ((int)tile.transform.position.z + 0.5) - 1] = tile;
        }*/
        InitializePuzzle();
    }

    void InitializePuzzle()
    {
        emptyTile = tiles[3, 3];
        for (int i = 0; i < 0; i++)
        {
            Tile randomNeighbour = emptyTile.GetRandomNeighbourTile(tiles);
            if (randomNeighbour != null)
            {
                SwapTiles(emptyTile, randomNeighbour);
                emptyTile = randomNeighbour;
            }
 
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Calculate which tile the player is trying to move
            Vector3 mousePosition = Input.mousePosition;
            float depth = 8.0f; // Adjust this value to the height of your tiles
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, depth));
            Debug.Log(worldPosition);
            Tile clickedTile = GetTileAtPosition(worldPosition);
            Debug.Log("old empty tile pos ");
            Debug.Log(emptyTile.transform.position);
            // Check if the clicked tile is adjacent to the empty tile
            if (clickedTile != null && clickedTile.CanMoveTo(emptyTile.transform.position))
            {
                tempTile = clickedTile;
                clickedTile.SwapWith(emptyTile);
                emptyTile = tempTile;
                Debug.Log("new empty tile pos ");
                Debug.Log(emptyTile.transform.position);
            }
        }
    }

    bool IsPuzzleSolved()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int z = 0; z < 4; z++)
            {
                if (tiles[x, z].transform.position != new Vector3(x + 0.5f, 0, z + 0.5f))
                {
                    return false;
                }
            }
        }
        return true;
    }

    Tile GetRandomTile()
    {
        int x = Random.Range(0, 4);
        int z = Random.Range(0, 4);
        return tiles[x, z];
    }

    void SwapTiles(Tile tile1, Tile tile2)
    {
        Vector3 tempPosition = tile1.transform.position;
        tile1.transform.position = tile2.transform.position;
        tile2.transform.position = tempPosition;
    }

    Tile GetTileAtPosition(Vector3 position)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int z = 0; z < 4; z++)
            {
                if ((Mathf.Abs(tiles[x, z].transform.position.x - position.x) < 0.5f) && (Mathf.Abs(tiles[x, z].transform.position.z - position.z) < 0.5f))
                {
                    return tiles[x, z];
                }
            }
        }
        return null;
    }
}