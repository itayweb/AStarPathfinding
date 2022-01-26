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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindPath(Node start, Node end, int width, int height, Node[,] gridArray)
    {
        List<Node> openList = new List<Node>() {start};
        List<Node> closeList = new List<Node>();

        while (openList.Any())
        {
            Node current = openList[0];
            foreach (var node in openList.Where(node => node.GetFCost() <= current.GetFCost() && node.GetHCost() < current.GetHCost()))
                current = node;
            closeList.Add(current);
            openList.Remove(current);
            current.FindNeighbors(width, height, gridArray);
            foreach (var neighbor in current.GetNeightbors().Where(neighbor => neighbor.IsWalkable && !closeList.Contains(neighbor)))
            {
                bool isNeedSearch = openList.Contains(neighbor);
                int costToNeighbor = neighbor.GetGCost() + neighbor.GetDistance(end);
                if (!isNeedSearch || costToNeighbor < neighbor.GetGCost())
                {
                    neighbor.SetG(costToNeighbor);
                    neighbor.Parent = current;
                    if (!isNeedSearch)
                    {
                        neighbor.SetH(neighbor.GetDistance(end));
                        openList.Add(neighbor);
                    }
                }
            }
        }
    }
}
