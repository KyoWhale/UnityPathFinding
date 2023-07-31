using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    Start, Space, OpenList, Obstacle, Destination
}

public class MapCell : MonoBehaviour
{
    public CellType type = CellType.Space;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void OnMouseEnter()
    {
        if (Input.GetMouseButton(0) == false)
        {
            return;
        }

        ToggleObstacle();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) == false)
        {
            return;
        }

        ToggleObstacle();
    }

    private void ToggleObstacle()
    {
        if (type == CellType.Space)
        {
            type = CellType.Obstacle;
            ColorCell();
        }
        else
        {
            type = CellType.Space;
            ColorCell();
        }
    }

    public void ColorCell()
    {
        switch (type)
        {
            case CellType.Start:
            case CellType.OpenList:
                _meshRenderer.material.color = Color.cyan;
                break;
            case CellType.Destination:
                _meshRenderer.material.color = Color.red;
                break;
            case CellType.Space:
            default:
                _meshRenderer.material.color = Color.white;
                break;
        }
    }
}
