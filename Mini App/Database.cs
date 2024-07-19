using System.Xml;
public static class Database
{
    private static readonly string filePath = "database.json";

    public static object JsonConvert { get; private set; }

    public static void SaveData(List<Classroom> classrooms)
    {
        var json = JsonConvert.SerializeObject(
            classrooms,
            Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public static List<Classroom> LoadData()
    {
        if (!File.Exists(filePath))
        {
            return new List<Classroom>();
        }

        var json = File.ReadAllText(filePath);
        return NewMethod(json);
    }

    private static List<Classroom> NewMethod(string json)
    {
        return JsonConvert.DeserializeObject<List<Classroom>>(json);
    }
}