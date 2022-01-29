using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Start,
    End,
    Obstacle,
    Ground,
    Path
};

public class TilemapSystem : MonoBehaviour
{
    // An array of scriptable objects of all tile types
    [SerializeField] private TilemapTile[] tiles;
    [SerializeField] private LayerMask tilemapLayer;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TMP_InputField widthInputField;
    [SerializeField] private TMP_InputField heightInputField;
    private TileType currentTile;
    private int widthLen;
    private int heightLen;
    private Node[,] gridArray;
    private Node startNode, endNode;
    
    void Start()
    {
        currentTile = TileType.Ground;
    }
    
    void Update()
    {
        // Detect mouse position and placing the selected tile in the tile map
        PlaceTile();
    }

    private void PlaceTile()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
                Mathf.Infinity, tilemapLayer);
            if (hit2D.collider != null)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var cellPos = tilemap.WorldToCell(mousePos);
                ChangeTile(cellPos);
            }
        }
    }

    private void ChangeTile(Vector3Int cellPos)
    {
        if (currentTile == TileType.Start || currentTile == TileType.End)
        {
            bool startExist = false;
            bool endExist = false;
            Vector3Int oldPos = new Vector3Int(0, 0, 0);
            for (int x = 0; x < widthLen; x++)
            {
                for (int y = 0; y < heightLen; y++)
                {
                    if (currentTile == TileType.End)
                    {
                        if (tiles[(int) currentTile] == tilemap.GetTile(new Vector3Int(x, y, 0)))
                        {
                            endExist = true;
                            oldPos = new Vector3Int(x, y, 0);
                            break;
                        }
                    }
                    else if (currentTile == TileType.Start)
                    {
                        if (tiles[(int) currentTile] == tilemap.GetTile(new Vector3Int(x, y, 0)))
                        {
                            startExist = true;
                            oldPos = new Vector3Int(x, y, 0);
                            break;
                        }
                    }
                }
            }

            if (cellPos != oldPos && (startExist || endExist))
            {
                tilemap.SetTile(cellPos, tiles[(int)currentTile]);
                tilemap.SetTile(oldPos, tiles[(int)TileType.Ground]);
            }
            else
            {
                tilemap.SetTile(cellPos, tiles[(int)currentTile]);
            }
            gridArray[cellPos.x, cellPos.y] = new Node(true, cellPos.x, cellPos.y);
            if (currentTile == TileType.End)
                endNode = gridArray[cellPos.x, cellPos.y];
            else if (currentTile == TileType.Start)
                startNode = gridArray[cellPos.x, cellPos.y];
        }
        else if (currentTile == TileType.Obstacle)
        {
            tilemap.SetTile(cellPos, tiles[(int)currentTile]);
            gridArray[cellPos.x, cellPos.y] = new Node(false, cellPos.x, cellPos.y);
        }
        else
        {
            tilemap.SetTile(cellPos, tiles[(int)currentTile]);
            gridArray[cellPos.x, cellPos.y] = new Node(true, cellPos.x, cellPos.y);
        }
    }

    // Generate the tile map grid according to the width and height input
    public void GenerateGrid()
    {
        gridArray = new Node[int.Parse(widthInputField.text), int.Parse(heightInputField.text)];
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tiles[3]);
                gridArray[x, y] = new Node(true, x, y);
            }
        }
        widthLen = Int32.Parse(widthInputField.text);
        heightLen = Int32.Parse(heightInputField.text);
        widthInputField.text = "";
        heightInputField.text = "";
    }
    
    // Reset the tile map grid
    public void ResetGrid()
    {
        tilemap.ClearAllTiles();
    }

    public void StartAlgorithm()
    {
        if (startNode == null || endNode == null)
            Debug.Log("Start node and End node not placed");
        else
        {
            List<Node> pathList = PathfindingSystem.Instance.FindPath(startNode, endNode, widthLen, heightLen, gridArray);
            if (pathList == null)
            {
                Debug.Log("Path not found!");
                return;
            }
            foreach (var t in pathList)
            {
                tilemap.SetTile(new Vector3Int(t.GetX(), t.GetY(), 0), tiles[(int)TileType.Path]);
            }
        }
    }

    #region ChangeCurrentTile
    public void SelectStartTile()
    {
        currentTile = TileType.Start;
    }
    
    public void SelectEndTile()
    {
        currentTile = TileType.End;
    }
    
    public void SelectObstacleTile()
    {
        currentTile = TileType.Obstacle;
    }
    
    public void SelectGroundTile()
    {
        currentTile = TileType.Ground;
    }
    #endregion
}
