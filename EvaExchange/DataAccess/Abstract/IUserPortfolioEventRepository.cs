﻿
using Core.DataAccess;
using Entities.Concrete;
using System;
namespace DataAccess.Abstract
{
    public interface IUserPortfolioEventRepository : IEntityRepository<UserPortfolioEvent>
    {
    }
}