
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class ShareRateHistoryRepository : EfEntityRepositoryBase<ShareRateHistory, ProjectDbContext>, IShareRateHistoryRepository
    {
        public ShareRateHistoryRepository(ProjectDbContext context) : base(context)
        {
        }

        public async Task<ShareRateHistory> GetLastShareRateHistoryByShareIdAsync(Guid Id)
        {
            return await Query()
                .Where(w => w.ShareId == Id)
                .OrderByDescending(o => o.UpdatedDate)
                .FirstOrDefaultAsync();
        }
    }
}
