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
        lastNode = OnNodeStand();
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
