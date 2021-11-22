namespace Libjector.Core.Bindings;

public record ProcessItemBinding
{

    public int Id { get; init; }
    public string Name { get; init; }
    public string Architecture { get; init; }
    public string Path { get; init; }

}