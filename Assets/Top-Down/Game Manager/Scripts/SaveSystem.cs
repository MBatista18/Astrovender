using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string path = Application.persistentDataPath + "/data.bin";

    public static void SaveToFile(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadFromFile()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void DeleteData()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted from " + path);
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }
    }

    public static bool SaveFileExists() => File.Exists(path);
}
