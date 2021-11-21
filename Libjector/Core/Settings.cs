using System;
using System.IO;
using System.Text.Json;

namespace Libjector.Core;

public class Settings
{

    private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libjector.settings");

    public string[] LibraryPaths { get; set; } = Array.Empty<string>();

    public void Save()
    {
        File.WriteAllText(FilePath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
    }

    public static Settings Load()
    {
        return !File.Exists(FilePath)
            ? new Settings()
            : JsonSerializer.Deserialize<Settings>(File.ReadAllText(FilePath));
    }

}