namespace FreshBack.Domain.Models.Abstraction;

public abstract class BaseImageModel<T> : BaseModel<T>
{
    public string ImagePath { get; set; } = default!;
}
