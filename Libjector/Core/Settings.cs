using System;
using System.IO;
using System.Text.Json;

namespace Libjector.Core;

public class Settings
{

    private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libjector.settings.json");

    public string[] SavedDllPaths { get; set; } = Array.Empty<string>();
    public int SavedMethodIndex { get; set; }
    public bool SavedHideDllFlagChecked { get; set; }
    public bool SavedRandomizeHeaderFlagChecked { get; set; }
    public bool SavedRandomizeNameFlagChecked { get; set; } = true;

    public void Save()
    {
        var fileContent = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, fileContent);
    }

    public static Settings Load()
    {
        if (!File.Exists(FilePath))
            return new Settings();
        var fileContent = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<Settings>(fileContent);
    }

}