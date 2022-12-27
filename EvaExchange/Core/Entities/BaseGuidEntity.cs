using System;

namespace Core.Entities
{
    public class BaseGuidEntity : IEntity
    {
        public virtual Guid Id { get; set; }
    }
}
