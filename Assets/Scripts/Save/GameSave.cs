using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

public static class GameSave
{
    public static async Task Save(string path, object objectToSave)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        await Task.Run(() =>
        {
            FileStream file = File.Create(path);
            binaryFormatter.Serialize(file, objectToSave);
            file.Close();
        });
    }

    public static async Task<T> Load<T>(string path) where T : class
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        T data = null;

        await Task.Run(() =>
        {
            FileStream file = File.Open(path, FileMode.Open);
            data = binaryFormatter.Deserialize(file) as T;
            file.Close();
        });

        return data;
    }
}