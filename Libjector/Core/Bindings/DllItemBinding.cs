namespace Libjector.Core.Bindings;

public record DllItemBinding
{

    public string Name { get; init; }
    public string Architecture { get; init; }
    public string Path { get; init; }

}