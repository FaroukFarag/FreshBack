namespace FreshBack.Domain.Shared.Attributs;

[AttributeUsage(AttributeTargets.Property)]
public class FilterPropertyAttribute(string entityPropertyName) : Attribute
{
    public string EntityPropertyName { get; } = entityPropertyName;
}
