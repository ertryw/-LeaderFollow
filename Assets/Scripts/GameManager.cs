using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ChooseLeaderPanel chooseLeaderPanel;

    public static GameManager Instance { get; private set; }
    private UnitController[] Units { get; set; }
    private UnitController Player { get; set; }

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
        Units = FindObjectsOfType<UnitController>();
        Player = Units.FirstOrDefault();
        chooseLeaderPanel.SetLeader(Player.gameObject);

        foreach (UnitController controller in Units)
        {
            chooseLeaderPanel.AddButton(controller);
        }


    }

}
