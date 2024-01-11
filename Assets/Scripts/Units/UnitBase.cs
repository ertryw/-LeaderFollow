using UnityEngine;

public class UnitBase : MonoBehaviour
{
    private GridMesh grid;
    private Node lastNode;

    public delegate void OnNodeChange(Node prevNode, Node newNode);
    public event OnNodeChange onNodeChange;

    void Awake()
    {
        grid = FindFirstObjectByType<GridMesh>();
        lastNode = GetNode();
    }

    public Node GetNode()
    {
        return grid.NotWalkableFromWorld(transform.position);
    }

    private void UpdateNode()
    {
        Node currentNode = grid.NotWalkableFromWorld(transform.position);

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
