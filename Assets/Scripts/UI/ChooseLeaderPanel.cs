using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLeaderPanel : MonoBehaviour
{
    [SerializeField]
    private Button buttonUIPrefab;

    private LeaderController leader;
    private List<Button> buttons = new List<Button>();

    public void SetLeader(GameObject unit)
    {
        leader = unit.AddComponent(typeof(LeaderController)) as LeaderController;
    }

    private void ResetLeader()
    {
        leader.gameObject.GetComponent<UnitController>().SwitchToDefault();
        Destroy(leader);
    }

    public void ResetButtons()
    {
        foreach (Button button in buttons)
        {
            Destroy(button.gameObject);
        }

        buttons.Clear();
    }

    public void AddButton(UnitController unit)
    {
        var button  = Instantiate(buttonUIPrefab);
        button.transform.SetParent(transform);
        
        button.onClick.AddListener(() => 
        {
            ResetLeader();
            SetLeader(unit.gameObject);
        });

        buttons.Add(button);
    }

}
