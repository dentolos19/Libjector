using System.IO;
using System.Text.Json;

namespace Libjector.Core;

public class Settings
{
    private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libjector.settings.json");

    public string[] DllPaths { get; set; } = [];
    public int MethodIndex { get; set; }
    public bool IsHideDllFlagChecked { get; set; }
    public bool IsRandomizeHeadersFlagChecked { get; set; }
    public bool IsRandomizeNameFlagChecked { get; set; } = true;
    public bool IsDiscardHeadersChecked { get; set; }
    public bool IsSkipInitializationRoutinesChecked { get; set; }

    public void Save()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public static Settings Load()
    {
        if (!File.Exists(FilePath))
            return new Settings();
        try
        {
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<Settings>(json)!;
        }
        catch
        {
            return new Settings();
        }
    }
}