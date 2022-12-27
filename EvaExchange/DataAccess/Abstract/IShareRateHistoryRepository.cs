
using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IShareRateHistoryRepository : IEntityRepository<ShareRateHistory>
    {
        Task<ShareRateHistory> GetLastShareRateHistoryByShareIdAsync(Guid Id);
    }
}