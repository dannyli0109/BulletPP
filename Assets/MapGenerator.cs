using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public List<GameObject> tilesPrefab;
    public int directions = 4;
    public int tileWidth;
    public int tileHeight;
    public float offsetX;
    public float offsetY;
    public float offsetZ;
    public TileBase tileBase;

    int[] dirX = { 0, 1, 0, -1, -1, 1, 1, -1 };
    int[] dirY = { -1, 0, 1, 0, -1, -1, 1, 1 };

    int[] directionMask =
    {
        1, 2, 4, 8, 16, 32, 64, 128
    };

    int[] indices = {
          0,   1,   2,   3,  4, 5,  6, 7,   8,  9, 10, 11, 12, 13, 14, 15,
         16,   1,  17,   3, 18, 5, 19, 7,   8,  9, 10, 11, 12, 13, 14, 15,
         20,   1,   2,   3, 21, 5,  6, 7,  22,  9, 10, 11, 23, 13, 14, 15,
         24,   1,  17,   3, 25, 5, 19, 7,  22,  9, 10, 11, 23, 13, 14, 15,
         26,  27,   2,   3,  4, 5,  6, 7,  28, 29, 10, 11, 12, 13, 14, 15,
         30,  27,  17,   3, 18, 5, 19, 7,  28, 29, 10, 11, 12, 13, 14, 15,
         31,  27,   2,   3, 21, 5,  6, 7, 32, 29, 10, 11, 23, 13, 14, 15,
        33,  27,  17,   3, 25, 5, 19, 7, 32, 29, 10, 11, 23, 13, 14, 15,
        34, 35, 36, 37,  4, 5,  6, 7,   8,  9, 10, 11, 12, 13, 14, 15,
        38, 35, 39, 37, 18, 5, 19, 7,   8,  9, 10, 11, 12, 13, 14, 15,
        40, 35, 36, 37, 21, 5,  6, 7,  22,  9, 10, 11, 23, 13, 14, 15,
        41, 35, 39, 37, 25, 5, 19, 7,  22,  9, 10, 11, 23, 13, 14, 15,
        42, 43, 36, 37,  4, 5,  6, 7,  28, 29, 10, 11, 12, 13, 14, 15,
        44, 43, 39, 37, 18, 5, 19, 7,  28, 29, 10, 11, 12, 13, 14, 15,
        45, 43, 36, 37, 21, 5,  6, 7, 32, 29, 10, 11, 23, 13, 14, 15,
        46, 43, 39, 37, 25, 5, 19, 7, 32, 29, 10, 11, 23, 13, 14, 15
    };

    // Start is called before the first frame update
    void Start()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        //tilemap.SetTile(new Vector3Int(0, 0, 0), tileBase);


        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        GameObject level = new GameObject("Level");

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];


                int mask = 0;
                if (tile != null)
                {
                    for (int i = 0; i < directions; i++)
                    {
                        int newX = x + dirX[i];
                        int newY = y + dirY[i];
                        int index = newX + newY * bounds.size.x;
                        if (newX < 0 || newY < 0 || newX >= bounds.size.x || newY >= bounds.size.y)
                        {
                            mask |= directionMask[i];
                            continue;
                        }
                        TileBase newTile = allTiles[index];
                        if (newTile == null)
                        {
                            mask |= directionMask[i];
                        }
                    }
                    GameObject tileObject = Instantiate(
                        tilesPrefab[indices[mask]],
                        new Vector3(
                            x * tileWidth + offsetX + bounds.min.x * tileWidth,
                            offsetY,
                            -y * tileHeight + offsetZ - bounds.min.y * tileHeight),
                        Quaternion.identity);
                    tileObject.transform.SetParent(level.transform);
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
