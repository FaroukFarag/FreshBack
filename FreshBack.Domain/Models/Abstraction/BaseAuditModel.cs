namespace FreshBack.Domain.Models.Abstraction;

public abstract class BaseAuditModel<TPrimaryKey> : BaseModel<TPrimaryKey>
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public TPrimaryKey CreatedBy { get; set; } = default!;
    public DateTime LastModifiedAt { get; set; } = DateTime.Now;
    public TPrimaryKey LastModifiedBy { get; set; } = default!;
}
