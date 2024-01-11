using UnityEngine;

public class LeaderController : MonoBehaviour
{
    private Camera camera;
    private UnitBase unit;
    private PathFind pathFinding;
    private UnitController unitController;

    public delegate void OnLead(Vector3 position);
    public static event OnLead onLead;

    // Start is called before the first frame update
    void Awake()
    {
        unit = GetComponent<UnitBase>();
        pathFinding = GetComponent<PathFind>();
        unitController = GetComponent<UnitController>();        
        camera = Camera.main;
    }

    private void OnEnable()
    {
        unit.onNodeChange += Lead;
        unitController.SwitchToLeader();
    }

    private void OnDisable()
    {
        unit.onNodeChange -= Lead;
        unitController.SwitchToDefault();
    }

    private void Lead(Node prevNode, Node newNode)
    {
        onLead?.Invoke(prevNode.WorldPosition);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
                pathFinding.StartPathFinding(new Vector3(hit.point.x, 4.0f, hit.point.z));
        }
    }


}
