using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static IPathfinding;


public class AStar : IPathfinding
{
    private bool running;
    private CancellationTokenSource token;

    private GridMesh grid;
    public event OnPathFound onPathFound;

    public AStar(GridMesh gridMesh)
    {
        grid = gridMesh;
    }

    public void Start(Vector3 startPos, Vector3 targetPos)
    {
        if (running == true)
            return;

        token = new CancellationTokenSource();
        FindPath(startPos, targetPos);
    }

    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        running = true;
        while (openSet.Count > 0)
        {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < node.FCost || openSet[i].FCost == node.FCost)
                {
                    if (openSet[i].HCost < node.HCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);
         
            if (node == targetNode)
            {
                onPathFound?.Invoke(RetracePath(startNode, targetNode));
                break;
            }

            foreach (Node neighbour in grid.GetNeighbours(node))
            {
                if (!neighbour.Walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.GCost + node.GetDistance(neighbour);
                if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newCostToNeighbour;
                    neighbour.HCost = neighbour.GetDistance(targetNode);
                    neighbour.Parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        running = false;
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path.Select(x => x.WorldPosition).ToArray();
    }
}
