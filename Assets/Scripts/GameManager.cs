using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject unitPrefab;

    [SerializeField]
    private ChooseLeaderPanel chooseLeaderPanel;

    private string savePath = @"/save.data";

    public static GameManager Instance { get; private set; }
    private List<UnitController> Units { get; set; }
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
        Units = FindObjectsOfType<UnitController>().ToList();
        Player = Units.FirstOrDefault();
        chooseLeaderPanel.SetLeader(Player.gameObject);

        foreach (UnitController unit in Units)
        {
            chooseLeaderPanel.AddButton(unit);
        }
    }

    public void Save()
    {
        UnitData[] unitsData = Units.Select(x => x.GetData()).ToArray();
        SaveFileWithBinaryFormater(Application.persistentDataPath + savePath, unitsData);
    }

    public void Load()
    {
        UnitData[] unitsData = LoadFileWithBinaryFormater<UnitData[]>(Application.persistentDataPath + savePath);

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
            {
                Player = unit;
                chooseLeaderPanel.SetLeader(unitObject);
            }

            Units.Add(unit);
            chooseLeaderPanel.AddButton(unit);
        }
    }

    public void SaveFileWithBinaryFormater(string path, object objectToSave)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(path);
        binaryFormatter.Serialize(file, objectToSave);
        file.Close();
    }

    public T LoadFileWithBinaryFormater<T>(string path) where T : class
    {
        try
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            T data = binaryFormatter.Deserialize(file) as T;
            file.Close();
            return data;
        }
        catch
        {
            return default(T);
        }
    }

}
