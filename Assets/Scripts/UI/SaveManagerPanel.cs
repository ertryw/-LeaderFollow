using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SaveManagerPanel : MonoBehaviour
{
    private string savePath = @"/save.data";

    [SerializeField]
    private Button loadButton;

    [SerializeField]
    private UnityEvent<UnitData[]> onLoad;

    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.persistentDataPath + savePath;
        CheckSave();       
    }

    private bool CheckSave()
    {
        if (SaveExist(savePath))
        {
            loadButton.interactable = true;
            return true;
        }
        else
        {
            loadButton.interactable = false;
            return false;
        }
    }

    public bool SaveExist(string path)
    {
        return File.Exists(path);
    }

    public async void Save()
    {
        UnitData[] unitsData = GameManager.Instance.Units.Select(x => x.GetData()).ToArray();
        await GameSave.Save(savePath, unitsData);
        CheckSave();
    }

    public async void Load()
    {
        if (CheckSave())
        {
            UnitData[] unitsData = await GameSave.Load<UnitData[]>(savePath);
            onLoad.Invoke(unitsData);
        }
    }

}
