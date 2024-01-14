using UnityEngine;

public class LeaderController : MonoBehaviour
{
    private Camera mainCamera;
    private UnitBase unit;
    private UnitController unitController;

    public delegate void OnLead(Vector3 position);
    public static event OnLead onLead;

    public static event UnitController.OnStaminaChange leaderStaminaChange;

    void Awake()
    {
        unit = GetComponent<UnitBase>();
        unitController = GetComponent<UnitController>();        
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        unit.onNodeChange += Lead;
        unitController.onStaminaChange += StaminaChange;
        unitController.SwitchToLeader();
    }

    private void OnDisable()
    {
        unit.onNodeChange -= Lead;
        unitController.onStaminaChange -= StaminaChange;
        unitController.SwitchToDefault();
    }

    private void Lead(Node prevNode, Node newNode)
    {
        onLead?.Invoke(prevNode.WorldPosition);
    }

    private void StaminaChange(float stamina)
    {
        leaderStaminaChange?.Invoke(stamina);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
                unit.StartPathFinding(transform.position, new Vector3(hit.point.x, 4.0f, hit.point.z));
        }
    }


}
