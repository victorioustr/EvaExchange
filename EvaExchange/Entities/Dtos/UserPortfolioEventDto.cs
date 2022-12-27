using Core.Entities;
using Entities.Enums;
using System;

namespace Entities.Concrete
{
    public class UserPortfolioEventDto : IDto
    {
        public Guid Id { get; set; }
        public Guid UserPortfolioId { get; set; }
        public UserPortfolioEventType UserPortfolioEventType { get; set; }
        public int Lot { get; set; }
        public decimal Rate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedUser { get; set; }
    }
}
