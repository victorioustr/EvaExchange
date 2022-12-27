using Core.Entities;
using System;

namespace Entities.Concrete
{
    public class ShareRateHistory : BaseGuidEntity
    {
        public Guid ShareId { get; set; }
        public decimal Rate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual Share Share { get; set; }
    }
}
