namespace FreshBack.Domain.Models.Abstraction;

public abstract class BaseImageAuditModel<TPrimaryKey> : BaseAuditModel<TPrimaryKey>
{
    public string ImagePath { get; set; } = default!;
}
