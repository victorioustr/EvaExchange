using Core.Entities;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;

namespace Entities.Concrete
{
    public class UserPortfolio : AuditableEntity
    {
        public UserPortfolio()
        {
            UserPortfolioEvents = new HashSet<UserPortfolioEvent>();
        }

        public int UserId { get; set; }
        public Guid ShareId { get; set; }
        public int Lot { get; set; }

        public virtual Share Share { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<UserPortfolioEvent> UserPortfolioEvents { get; set; }
    }
}
