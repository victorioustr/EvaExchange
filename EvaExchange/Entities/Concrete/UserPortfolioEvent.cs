using Core.Entities;
using Entities.Enums;
using System;

namespace Entities.Concrete
{
    public class UserPortfolioEvent : AuditableEntity
    {
        public Guid UserPortfolioId { get; set; }
        public UserPortfolioEventType UserPortfolioEventType { get; set; }
        public int Lot { get; set; }
        public decimal Rate { get; set; }

        public virtual UserPortfolio UserPortfolio { get; set; }
    }
}
