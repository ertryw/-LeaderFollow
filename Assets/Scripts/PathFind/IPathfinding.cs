using UnityEngine;

public interface IPathfinding
{
    public delegate void OnPathFound(Vector3[] path);
    event OnPathFound onPathFound;
    void Start(Vector3 start, Vector3 target);

}

public class PathFindingFactory
{
    public static IPathfinding GetPathfinding(GridMesh gridMesh)
    {
        // Implement other path finding algoritms

        return new AStar(gridMesh);
    }
}
