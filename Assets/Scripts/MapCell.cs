using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CellType
{
    Start, Space, OpenList, Obstacle, Destination, Path, Visited, Searching
}

public class MapCell : MonoBehaviour
{
    public CellType type = CellType.Space;
    public int x { get; set; }
    public int y { get; set; }

    private MeshRenderer _meshRenderer;
    private TMP_Text _text;

    private void Awake()
    {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        _text = gameObject.GetComponentInChildren<TMP_Text>();
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
        if (Input.GetMouseButtonDown(0))
        {
            ToggleObstacle();
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            SetType(CellType.Start);
            return;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SetType(CellType.Destination);
            return;
        }
    }

    private void ToggleObstacle()
    {
        if (type == CellType.Space)
        {
            type = CellType.Obstacle;
        }
        else
        {
            type = CellType.Space;
        }
        ColorCell();
    }

    public void SetType(CellType newType)
    {
        if (this.type == newType)
        {
            return;
        }
        
        if (newType == CellType.Start)
        {
            if (Map.instance.startCell == null)
            {
                Map.instance.startCell = this;
            }
            else
            {
                Map.instance.startCell.SetType(CellType.Space);
                Map.instance.startCell = this;
            }
        }
        else if (newType == CellType.Destination)
        {
            if (Map.instance.destinationCell == null)
            {
                Map.instance.destinationCell = this;
            }
            else
            {
                Map.instance.destinationCell.SetType(CellType.Space);
                Map.instance.destinationCell = this;
            }
        }
        
        this.type = newType;
        ColorCell();
    }

    public void ColorCell()
    {
        switch (type)
        {
            case CellType.Start:
            case CellType.Searching:
            case CellType.Path:
                _meshRenderer.material.color = Color.green;
                break;
            case CellType.Visited:
                _meshRenderer.material.color = new Color(.5f, .7f, .5f, 1);
                break;
            case CellType.OpenList:
                _meshRenderer.material.color = Color.cyan;
                break;
            case CellType.Obstacle:
                _meshRenderer.material.color = Color.black;
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

    public void SetText(string text)
    {
        _text.text = text;
    }
}
