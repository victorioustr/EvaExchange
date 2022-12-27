using Core.Entities;
using System;

namespace Entities.Dtos
{
    public class ShareRateHistoryDto : IDto
    {
        public Guid Id { get; set; }
        public Guid ShareId { get; set; }
        public decimal Rate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
