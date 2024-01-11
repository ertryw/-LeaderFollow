using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFind : MonoBehaviour
{
    private bool running;
    private GridMesh grid;
    public Vector3[] Path { get; set; }

    public delegate void OnPathFound();
    public event OnPathFound onPathFound;

    private void Awake()
    {
        grid = FindFirstObjectByType<GridMesh>();
    }

    public void StartPathFinding(Vector3 target)
    {
        if (running == true)
            return;

        running = false;
        StopCoroutine("FindPath");
        StartCoroutine(FindPath(transform.position, target));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        running = true;
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
 

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
                RetracePath(startNode, targetNode);
                onPathFound?.Invoke();
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
        yield return null;
    }

    Vector3[] WorldPath(List<Node> path)
    {
        return path.Select(x => x.WorldPosition).ToArray();
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();

        if (transform.name == "Player")
            grid.path = path;

        Path = WorldPath(path);
    }



}
