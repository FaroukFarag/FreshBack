namespace FreshBack.Domain.Models.Abstraction;

public abstract class BaseModel<TPrimaryKey>
{
    public TPrimaryKey Id { get; set; } = default!;
}
