
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using System;
using System.Linq;
namespace DataAccess.Concrete.EntityFramework
{
    public class ShareRepository : EfEntityRepositoryBase<Share, ProjectDbContext>, IShareRepository
    {
        public ShareRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
