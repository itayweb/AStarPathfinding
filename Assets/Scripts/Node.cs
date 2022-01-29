using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private int _fCost;
    private int _hCost;
    private int _gCost;
    private bool _isWalkable;
    private int _x;
    private int _y;
    private List<Node> _neighbors;
    private Node _parent;

    public Node(bool isWalkable, int x, int y)
    {
        this._isWalkable = isWalkable;
        this._gCost = 0;
        this._hCost = 0;
        this._fCost = 0;
        this._x = x;
        this._y = y;
    }

    private static bool InBound(int x, int y, int width, int height)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public void FindNeighbors(int width, int height, Node[,] gridArray)
    {
        List<Node> list = new List<Node>();
        if (InBound(this._x - 1, this._y, width, height))
        {
            if (gridArray[_x - 1, _y]._isWalkable)
                list.Add(gridArray[_x - 1, _y]);
        }
        if (InBound(this._x + 1, this._y, width, height))
        {
            if (gridArray[_x + 1, _y]._isWalkable)
                list.Add(gridArray[_x + 1, _y]);
        }
        if (InBound(this._x, this._y + 1, width, height))
        {
            if (gridArray[_x, _y + 1]._isWalkable)
                list.Add(gridArray[_x, _y + 1]);
        }
        // else if (InBound(this.x - 1, this.y + 1, width, height))
        // {
        //     if (gridArray[x - 1, y + 1].isWalkable)
        //         list.Add(gridArray[x - 1, y + 1]);
        // }
        if (InBound(this._x, this._y - 1, width, height))
        {
            if (gridArray[_x, _y - 1]._isWalkable)
                list.Add(gridArray[_x, _y - 1]);
        }
        this._neighbors = list;
    }

    public int GetDistance(Node neighbor)
    {
        return (int)Mathf.Sqrt(Mathf.Pow((neighbor._x - this._x), 2) + Mathf.Pow((neighbor._y - this._y), 2));
    }

    #region GettersAndSetters
    public void SetG(int value)
    {
        this._gCost = value;
        this._fCost = this._gCost + this._hCost;
    }

    public void SetH(int value)
    {
        this._hCost = value;
        this._fCost = this._gCost + this._hCost;
    }

    public int GetGCost()
    {
        return this._gCost;
    }

    public int GetHCost()
    {
        return this._hCost;
    }

    public int GetFCost()
    {
        return this._fCost;
    }

    public List<Node> GetNeightbors()
    {
        return this._neighbors;
    }

    public void SetIsWalkable(bool value)
    {
        _isWalkable = value;
    }

    public bool GetIsWalkable()
    {
        return this._isWalkable;
    }

    public void SetX(int value)
    {
        _x = value;
    }

    public int GetX()
    {
        return _x;
    }
    
    public void SetY(int value)
    {
        _y = value;
    }

    public int GetY()
    {
        return _y;
    }

    public void SetParent(Node value)
    {
        _parent = value;
    }

    public Node GetParent()
    {
        return _parent;
    }
    #endregion
}
