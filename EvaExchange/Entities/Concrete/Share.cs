using Core.Entities;
using System;
using System.Collections.Generic;

namespace Entities.Concrete
{
    public class Share : AuditableEntity
    {
        public Share()
        {
            ShareRateHistories = new HashSet<ShareRateHistory>();
            UserPortfolios = new HashSet<UserPortfolio>();
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }

        public virtual ICollection<ShareRateHistory> ShareRateHistories { get; set; }
        public virtual ICollection<UserPortfolio> UserPortfolios { get; set; }
    }
}
