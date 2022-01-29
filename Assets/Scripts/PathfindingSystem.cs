using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingSystem : MonoBehaviour
{
    public static PathfindingSystem Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public List<Node> FindPath(Node start, Node end, int width, int height, Node[,] gridArray)
    {
        List<Node> openList = new List<Node>() {start};
        List<Node> closeList = new List<Node>();

        while (openList.Any())
        {
            Node current = FindLowestFCost(openList);
            if (current == end)
                return GetPath(current);
            closeList.Add(current);
            openList.Remove(current);
            current.FindNeighbors(width, height, gridArray);
            foreach (var neighbor in current.GetNeightbors().Where(neighbor => neighbor.GetIsWalkable() && !closeList.Contains(neighbor)))
            {
                bool isNeedSearch = openList.Contains(neighbor);
                int costToNeighbor = neighbor.GetGCost() + neighbor.GetDistance(end);
                if (!isNeedSearch || costToNeighbor < neighbor.GetGCost())
                {
                    neighbor.SetG(costToNeighbor);
                    neighbor.SetParent(current);
                    if (!isNeedSearch)
                    {
                        neighbor.SetH(neighbor.GetDistance(end));
                        openList.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }

    private Node FindLowestFCost(List<Node> list)
    {
        Node lowest = list[0];
        foreach (var tNode in list)
        {
            if (tNode.GetFCost() < lowest.GetFCost())
                lowest = tNode;
        }
        return lowest;
    }

    private List<Node> GetPath(Node node)
    {
        List<Node> list = new List<Node>();
        while (node != null)
        {
            list.Add(node);
            node = node.GetParent();
        }
        return list;
    }
}
