using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameLogic : MonoBehaviour
{
    public Tile[,] tiles = new Tile[4, 4];
    private Tile emptyTile;
    bool winFlag = false;
    bool loseFlag = false;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    public TextMeshProUGUI moveText;
    public TextMeshProUGUI maxMoveText;
    private int moveCount = 0;
    public int maxMoves = 30;
    void Start()
    {
        maxMoveText.text = "Max moves: " + maxMoves;
        GameObject[] tileObjects = GameObject.FindGameObjectsWithTag("Tile");
        // Initialize the tiles array with the found GameObjects
        for (int i = 0, x,z; i < tileObjects.Length; i++)
        {
            Tile tile = tileObjects[i].GetComponent<Tile>();
            x = (int)((tile.transform.position.x + 0.5f) - 1);
            z = (int)((tile.transform.position.z + 0.5f) - 1);

            string spritePath;
            if (z + x * 4 + 1<10)
            {
                spritePath = "TileNumbers/" + (int)(z + x * 4 + 1) + "/" + Random.Range(0, 8);
            }
            else
            {
                spritePath = "TileNumbers/" + (int)(z + x * 4 + 1) + "/" + Random.Range(0, 4);
            }
            Sprite newSprite = Resources.Load<Sprite>(spritePath);
            if (newSprite != null)
            {
                tile.transform.Find("ImageSprite").GetComponent<SpriteRenderer>().sprite = newSprite;
            }
            /*else
            {
                Debug.Log("No sprite found ");

            }
            Debug.Log(spritePath);*/

            tiles[x, z] = tile;
        }
        InitializePuzzle();
    }

    void InitializePuzzle()
    {
        emptyTile = tiles[3, 3];
        for (int i = 0; i < 10; i++)
        {
            Tile randomNeighbour = emptyTile.GetRandomNeighbourTile(tiles);

            SwapTiles(emptyTile, randomNeighbour);
               
            

        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePosition = Input.mousePosition;
            float depth = 8.0f; // Adjust this value to the height 
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, depth));
            Tile clickedTile = GetTileAtPosition(worldPosition);
            if (clickedTile != null)
            {
                
                if (clickedTile != null && (clickedTile != emptyTile) && clickedTile.CanMoveTo(emptyTile.transform.position))
                {
                    moveCount++;
                    moveText.text = "Moves: " + moveCount;
                    
                    SwapTiles(clickedTile, emptyTile);
                    clickedTile.playTap();

                    RandomizeAllImages();
                }
            }
            
        }
        if (moveCount > maxMoves && winFlag == false && loseFlag == false)
        {
            loseFlag = true;
            loseText.text = "You fail.\nAwait external assistance.";
            loseText.gameObject.SetActive(true);
        }
        if (IsPuzzleSolved() && winFlag==false && loseFlag == false)
        {
            winFlag = true;
            winText.text = "You pass.\nTotal moves: " + moveCount;
            winText.gameObject.SetActive(true);
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
    void RandomizeAllImages()
    {
        for (int i = 0, x, z; i < tiles.Length; i++)
        {
            Tile tile = tiles[i/4, i%4];
            x = i / 4;
            z = i % 4;

            string spritePath;
            if (z + x * 4 + 1 < 10)
            {
                spritePath = "TileNumbers/" + (int)(z + x * 4 + 1) + "/" + Random.Range(0, 8);
            }
            else
            {
                spritePath = "TileNumbers/" + (int)(z + x * 4 + 1) + "/" + Random.Range(0, 4);
            }
            /*if(z + x * 4 + 1<10)*/
            Sprite newSprite = Resources.Load<Sprite>(spritePath);
            if (newSprite != null)
            {
                tile.transform.Find("ImageSprite").GetComponent<SpriteRenderer>().sprite = newSprite;
            }
            

        }
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
                if ((Mathf.Abs(tiles[x, z].transform.position.x - position.x) < 0.45f) && (Mathf.Abs(tiles[x, z].transform.position.z - position.z) < 0.45f))
                {
                    return tiles[x, z];
                }
            }
        }
        return null;
    }
}