using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject MapTile;

    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField] private int minPathLength = 30;

    public static List<GameObject> mapTiles = new List<GameObject>();
    public static List<GameObject> pathTiles = new List<GameObject>();

    public static GameObject startTile;
    public static GameObject endTile;

    private GameObject currentTile;
    private int currentIndex;
    private int nextIndex;

    public Color pathColor;

    public Color startColor;
    public Color endColor;

    public void Start()
    {
        generateMap();
    }

    private List<GameObject> getRightEdgeTiles()
    {
        List<GameObject> edgeTiles = new List<GameObject>();

        for (int i = mapWidth - 1; i < mapTiles.Count; i += mapWidth)
        {
            edgeTiles.Add(mapTiles[i]);
        }

        return edgeTiles;
    }

    //Checks if the cell at the indicated coordinates has no path tile
    public bool CellIsEmpty(int x, int y)
    {
        Vector3 desiredPos = new Vector3(x, y, 0.0f);
        foreach (GameObject tile in pathTiles)
        {
            if (tile.transform.position == desiredPos)
            {
                return false;
            }
        }
        return true;
    }

    //Generates the map layout
    private void generateMap()
    {
        //Starts putting the base tiles into a list
        for (int posY = 0; posY < mapHeight; posY++)
        {
            for (int posX = 0; posX < mapWidth; posX++)
            {
                GameObject newTile = Instantiate(MapTile);

                mapTiles.Add(newTile);

                newTile.transform.position = new Vector2(posX, posY);
            }
        }

        List<GameObject> rightEdgeTiles = getRightEdgeTiles();

        int loops = 0;
        int maxLoops = 10000;

        while (pathTiles.Count < minPathLength)
        {
            if (loops % (maxLoops / 10) == 0)
            {
                Debug.Log("Mark");
                minPathLength = minPathLength - 2;
            }
            else if (loops > maxLoops)
            {
                break;
            }

            int rand = Random.Range(0, mapHeight);
            startTile = rightEdgeTiles[rand];

            pathTiles = new List<GameObject>();

            int y = (int)startTile.transform.position.y;
            int x = (int)startTile.transform.position.x;

            currentTile = startTile;

            //Randomizes the path tiles and sets them by taking the base tiles into a new list

            while (x > 0)
            {
                bool validMode = false;

                while (!validMode)
                {
                    int move = Random.Range(0, 3);

                    if (move == 0 || x % 2 == 0 || x > (mapWidth - 2))
                    {
                        pathTiles.Add(currentTile);
                        currentIndex = mapTiles.IndexOf(currentTile);
                        nextIndex = currentIndex - 1;
                        currentTile = mapTiles[nextIndex];
                        validMode = true;
                    }
                    else if (move == 1 && CellIsEmpty(x, y + 1) && y < (mapHeight - 2))
                    {
                        pathTiles.Add(currentTile);
                        currentIndex = mapTiles.IndexOf(currentTile);
                        nextIndex = currentIndex + mapWidth;
                        currentTile = mapTiles[nextIndex];
                        validMode = true;
                    }
                    else if (move == 2 && CellIsEmpty(x, y - 1) && y > 2)
                    {
                        pathTiles.Add(currentTile);
                        currentIndex = mapTiles.IndexOf(currentTile);
                        nextIndex = currentIndex - mapWidth;
                        currentTile = mapTiles[nextIndex];
                        validMode = true;
                    }
                }

                x = (int)currentTile.transform.position.x;
                y = (int)currentTile.transform.position.y;
            }

            loops++;
            Debug.Log(loops);
        }

        endTile = pathTiles[pathTiles.Count - 1];

        foreach (GameObject obj in pathTiles)
        {
            obj.GetComponent<SpriteRenderer>().color = pathColor;
        }

        startTile.GetComponent<SpriteRenderer>().color = startColor;
        endTile.GetComponent<SpriteRenderer>().color = endColor;
    }
}