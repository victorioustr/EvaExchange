using System;

namespace Core.Entities
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }
        int CreatedUser { get; set; }
        DateTime? UpdatedDate { get; set; }
        int? UpdatedUser { get; set; }
    }
}
