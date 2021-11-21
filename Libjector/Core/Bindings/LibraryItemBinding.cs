namespace Libjector.Core.Bindings;

public record LibraryItemBinding
{

    public string Name { get; init; }
    public string Architecture { get; init; }
    public string Path { get; init; }

}