using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject unitPrefab;

    [SerializeField]
    private ChooseLeaderPanel chooseLeaderPanel;

    public static GameManager Instance { get; private set; }
    public List<UnitController> Units { get; set; }
    public UnitController Leader { get; set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Units = FindObjectsOfType<UnitController>().ToList();
        SetLeader(Units.FirstOrDefault());

        foreach (UnitController unit in Units)
        {
            chooseLeaderPanel.AddButton(unit);
        }
    }

    private void SetLeader(UnitController unit)
    {
        Leader = unit;
        chooseLeaderPanel.SetLeader(unit.gameObject);
    }

    public void OnLoad(UnitData[] unitsData)
    {

        foreach (UnitController unit in Units)
        {
            Destroy(unit.gameObject);
        }

        Units.Clear();
        chooseLeaderPanel.ResetButtons();

        foreach (UnitData unitData in unitsData)
        {
            Vector3 position = new Vector3(unitData.x, unitData.y, unitData.z);
            Quaternion rotation = Quaternion.Euler(unitData.rX, unitData.rY, unitData.rZ);
            GameObject unitObject = Instantiate(unitPrefab, position, rotation);
            UnitController unit = unitObject.GetComponent<UnitController>();

            unit.SetStats(unitData);

            if (unitData.leader)
                SetLeader(unit);

            Units.Add(unit);
            chooseLeaderPanel.AddButton(unit);
        }
    }
}
