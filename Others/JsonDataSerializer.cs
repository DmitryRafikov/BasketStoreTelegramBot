using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text;

public class JsonDataSerializer<T>
{
    public static T GetDeserializedObject(string path, T type)
    {
        return JsonConvert.DeserializeObject<T>(ReadData(path));
    }

    public static List<T> GetDeserializedObjectList(string path, List<T> type)
    {
        return JsonConvert.DeserializeObject<List<T>>(ReadData(path));
    }
    private static string ReadData(string path)
    {
        if (File.Exists(path))
            return File.ReadAllText(path, Encoding.GetEncoding("utf-8"));
        throw new ArgumentException("Файл не найден");
    }
    public static void SetToSerializedObject(string path, T type)
    {
        if (File.Exists(path))
        {
            JsonSerializer serializer = new JsonSerializer();
            using StreamWriter sw = new StreamWriter(path);
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, type);
        }
    }

}
