namespace FreshBack.Domain.Models.Abstraction;

public abstract class BaseImageModel<TPrimaryKey> : BaseModel<TPrimaryKey>
{
    public string ImagePath { get; set; } = default!;
}
