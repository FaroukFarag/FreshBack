namespace FreshBack.Domain.Lookups.Abstraction;

public abstract class BaseLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
