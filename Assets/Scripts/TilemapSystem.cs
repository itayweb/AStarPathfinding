using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private TilemapTile[] tiles;
    [SerializeField] private LayerMask tilemapLayer;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TMP_InputField widthInputField;
    [SerializeField] private TMP_InputField heightInputField;
    private TileType currentTile;
    
    void Start()
    {
        currentTile = TileType.Start;
    }
    
    void Update()
    {
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
                var cellPos = _tilemap.WorldToCell(mousePos);
                ChangeTile(cellPos);
            }
        }
    }

    private void ChangeTile(Vector3Int cellPos)
    {
        _tilemap.SetTile(cellPos, tiles[(int)currentTile]);
    }

    public void GenerateGrid()
    {
        int[,] gridArray = new int[int.Parse(widthInputField.text), int.Parse(heightInputField.text)];
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                _tilemap.SetTile(new Vector3Int(x, y, 0), tiles[3]);
            }
        }
    }
}
