using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int width = 10;
    public int height = 10;

    public MapCell[,] map;

    public void Start()
    {
        Camera.main.transform.position = new Vector3(height/2, 10, width/2);
        InitializeCellData();
        InitializeObstacles();
    }

    private void InitializeCellData()
    {
        if (map == null)
        {
            map = new MapCell[height, width];
            if (map.GetUpperBound(0) != height || map.GetUpperBound(1) != width)
            {
                for (int i = 0; i < transform.childCount; ++i)
                {
                    Destroy(transform.GetChild(i));
                }
            }

            for (int i = 0; i < height; ++i)
            {
                GameObject column = new GameObject($"{i}");
                column.transform.SetParent(transform);
                for (int j = 0; j < width; ++j)
                {
                    GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cell.name = $"({i},{j})";
                    cell.transform.SetParent(column.transform);
                    cell.transform.localPosition = new Vector3(i, 0, j);
                    cell.transform.localScale = Vector3.one * 0.9f;
                    map[i,j] = cell.AddComponent<MapCell>();
                }
            }
        }
    }

    private void InitializeObstacles()
    {
        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {

            }
        }
    }
}
