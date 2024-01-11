using UnityEngine;
using static IPathfinding;

public class UnitBase : MonoBehaviour
{
    private GridMesh grid;
    private Node lastNode;
    private IPathfinding pathFinding;

    public delegate void OnNodeChange(Node prevNode, Node newNode);
    public event OnNodeChange onNodeChange;

    void Awake()
    {
        grid = FindFirstObjectByType<GridMesh>();
        pathFinding = PathFindingFactory.GetPathfinding(grid);
        lastNode = OnNodeStand();
    }

    public void AddPathFound(OnPathFound pathfound)
    {
        pathFinding.onPathFound += pathfound;
    }

    public void RemovePathFound(OnPathFound pathfound)
    {
        pathFinding.onPathFound += pathfound;
    }

    public void StartPathFinding(Vector3 start, Vector3 target) 
    {
        pathFinding.Start(start, target);
    }

    public Node OnNodeStand()
    {
        Node node = grid.NodeFromWorldPoint(transform.position);
        node.Walkable = false;
        return node;
    }

    private void UpdateNode()
    {
        Node currentNode = OnNodeStand();

        if (currentNode != lastNode)
        {
            lastNode.Walkable = true;
            onNodeChange?.Invoke(lastNode, currentNode);

            lastNode = currentNode;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNode();
    }
}
