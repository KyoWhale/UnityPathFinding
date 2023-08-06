using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map instance;
    [SerializeField] private MapCell CellPrefab;
    [HideInInspector] public MapCell startCell;
    [HideInInspector] public MapCell destinationCell;

    public int width = 10;
    public int height = 10;

    public MapCell[,] map;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetCameraPosition();
        InitializeCellData();
        // InitializeObstacles();
    }

    private void SetCameraPosition()
    {
        Camera.main.transform.position = new Vector3(width / 2, 10, height / 2);
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
                    MapCell cell = Instantiate(CellPrefab, Vector3.zero, Quaternion.identity);
                    cell.name = $"({i},{j})";
                    cell.transform.SetParent(column.transform);
                    cell.transform.localPosition = new Vector3(j, 0, i);
                    cell.transform.localScale = Vector3.one * 0.9f;
                    map[i,j] = cell;
                    map[i,j].x = j;
                    map[i,j].y = i;
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

    public bool IsObstacle(int x, int y)
    {
        if (x < 0 || map.GetUpperBound(1) < x || y < 0 || map.GetUpperBound(0) < y)
        {
            return true;
        }

        return map[y, x].type == CellType.Obstacle;
    }
    
    public MapCell GetCell(int x, int y)
    {
        if (x < 0 || map.GetUpperBound(1) < x || y < 0 || map.GetUpperBound(0) < y)
        {
            return null;
        }

        return map[y, x];
    }

    public void SetCellType(int x, int y, CellType type)
    {
        var cell = GetCell(x, y);
        if (cell != null)
        {
            cell.SetType(type);
        }
    }

    public void SetCellText(int x, int y, string text)
    {
        var cell = GetCell(x, y);
        if (cell != null)
        {
            cell.SetText(text);
        }
    }
}
