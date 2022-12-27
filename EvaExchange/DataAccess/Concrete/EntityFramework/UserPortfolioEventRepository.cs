
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using System;
using System.Linq;
namespace DataAccess.Concrete.EntityFramework
{
    public class UserPortfolioEventRepository : EfEntityRepositoryBase<UserPortfolioEvent, ProjectDbContext>, IUserPortfolioEventRepository
    {
        public UserPortfolioEventRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
