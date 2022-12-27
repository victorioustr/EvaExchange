
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using System;
using System.Linq;
namespace DataAccess.Concrete.EntityFramework
{
    public class UserPortfolioRepository : EfEntityRepositoryBase<UserPortfolio, ProjectDbContext>, IUserPortfolioRepository
    {
        public UserPortfolioRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
