using System;

namespace Core.Entities
{
    public class AuditableEntity : BaseGuidEntity, IAuditableEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CreatedUser { get; set; }
        public int? UpdatedUser { get; set; }
    }
}
