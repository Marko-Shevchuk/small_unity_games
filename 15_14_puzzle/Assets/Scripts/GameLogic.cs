using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameLogic : MonoBehaviour
{
    public Tile[,] tiles = new Tile[4, 4];
    private Tile emptyTile;
    private Tile tempTile;
    void Start()
    {
        GameObject[] tileObjects = GameObject.FindGameObjectsWithTag("Tile");
        // Initialize the tiles array with the found GameObjects
        for (int i = 0; i < tileObjects.Length; i++)
        {
            Tile tile = tileObjects[i].GetComponent<Tile>();
            tiles[(int)((tile.transform.position.x + 0.5f) - 1), ((int)((tile.transform.position.z + 0.5f) - 1))] = tile;
            Debug.Log((int)((tile.transform.position.x + 0.5f) - 1));
            Debug.Log((int)((tile.transform.position.z + 0.5f) - 1));
            Debug.Log("TileNumbers/" + ((int)((tile.transform.position.z + 0.5f) - 1) + (int)((tile.transform.position.x + 0.5f) - 1) * 4 + 1) + "/" + Random.Range(0, 8) + ".jpg");
            if(((int)((tile.transform.position.z + 0.5f) - 1) + (int)((tile.transform.position.x + 0.5f) - 1) * 4 + 1)<10)
            {
                tiles[((int)((tile.transform.position.x + 0.5f) - 1)), (int)((tile.transform.position.z + 0.5f) - 1)].tileNumberImage.sprite = Resources.Load<Sprite>("TileNumbers/" + ((int)((tile.transform.position.z + 0.5f) - 1) + (int)((tile.transform.position.x + 0.5f) - 1) * 4 + 1) + "/" + Random.Range(0, 8) + ".jpg");

            }
            else
            {
                tiles[((int)((tile.transform.position.x + 0.5f) - 1)), (int)((tile.transform.position.z + 0.5f) - 1)].tileNumberImage.sprite = Resources.Load<Sprite>("TileNumbers/" + ((int)((tile.transform.position.z + 0.5f) - 1) + (int)((tile.transform.position.x + 0.5f) - 1) * 4 + 1) + "/" + Random.Range(0, 4) + ".jpg");

            }
        }
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

            Vector3 mousePosition = Input.mousePosition;
            float depth = 8.0f; // Adjust this value to the height 
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, depth));
            Debug.Log(worldPosition);
            Tile clickedTile = GetTileAtPosition(worldPosition);
            Debug.Log("old empty tile position ");
            Debug.Log(emptyTile.transform.position);
            if (clickedTile != null)
            {
                Debug.Log("clicked tile position ");
                Debug.Log(clickedTile.transform.position);
                if (clickedTile != null && clickedTile.CanMoveTo(emptyTile.transform.position))
                {
                    SwapTiles(clickedTile, emptyTile);
                    Debug.Log("new empty tile position ");
                    Debug.Log(emptyTile.transform.position);
                }
            }
            
        }
    }
    bool IsPuzzleSolved()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int z = 0; z < 4; z++)
            {
                if (tiles[x, z].transform.position != new Vector3(x + 0.5f, 0.25f, z + 0.5f))
                {
                    return false;
                }
            }
        }
        return true;
    }

    Tile GetRandomTile()
    {
        //only within the board
        int x = Random.Range(0, 4);
        int z = Random.Range(0, 4);
        return tiles[x, z];
    }
    void SwapTiles(Tile tile1, Tile tile2)
    {
        Vector3 tempPosition = tile1.transform.position;
        tile1.transform.position = tile2.transform.position;
        tile2.transform.position = tempPosition;
        //position only
    }

    Tile GetTileAtPosition(Vector3 position)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int z = 0; z < 4; z++)
            {
                if ((Mathf.Abs(tiles[x, z].transform.position.x - position.x) < 0.25f) && (Mathf.Abs(tiles[x, z].transform.position.z - position.z) < 0.25f))
                {
                    return tiles[x, z];
                }
            }
        }
        return null;
    }
}