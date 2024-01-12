using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class GameData
{
    public static UnitData[] unitsData;

    public static void Save()
    {

    }


    private static void SaveFileWithBinaryFormater(string path, object objectToSave)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(path);
        binaryFormatter.Serialize(file, objectToSave);
        file.Close();
    }

    public static T LoadFileWithBinaryFormater<T>(string path) where T : class
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