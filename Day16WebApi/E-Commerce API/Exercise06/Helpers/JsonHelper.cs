using System.Text.Json;

namespace Exercise06.Helpers
{
    public static class JsonHelper
    {
        // Read JSON file into a list of objects
        public static List<T> Read<T>(string path)
        {
            if (!File.Exists(path))
                return new List<T>();

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        // Write a list of objects into a JSON file
        public static void Write<T>(string path, List<T> data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, json);
        }
    }
}