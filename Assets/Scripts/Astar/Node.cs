using UnityEngine;

public class Node
{
    public Vector3 WorldPosition { get; }
    public float NodeRadius { get; }
    public bool Walkable { get; set; }
    public int GCost { get; set; }
    public int HCost { get; set; }
    public Vector2Int GridPosition { get; set; }
    public Node Parent { get; set; }
    public int FCost { get => GCost + HCost; }

    public Node(bool walkable, Vector3 worldPosition, Vector2Int gridPosition, float nodeRadius)
    {
        this.Walkable = walkable;
        this.WorldPosition = worldPosition;
        this.GridPosition = gridPosition;
        this.NodeRadius = nodeRadius;
    }

    private Vector2Int Distance(Node node)
    {
        return node.GridPosition - GridPosition;
    }

    private Vector2Int DistanceAbs(Node node)
    {
        Vector2Int distance = Distance(node);
        return new Vector2Int(Mathf.Abs(distance.x), Mathf.Abs(distance.y));
    }

    public int GetDistance(Node nodeB)
    {
        Vector2Int distance = DistanceAbs(nodeB);

        if (distance.x > distance.y)
            return 14 * distance.y + 10 * (distance.x - distance.y);

        return 14 * distance.x + 10 * (distance.y - distance.x);
    }




}
