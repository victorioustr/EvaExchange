
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.UserPortfolios.Queries
{

    public class GetUserPortfoliosByUserIdQuery : IRequest<IDataResult<IEnumerable<UserPortfolio>>>
    {
        public int UserId { get; set; }

        public class GetUserPortfoliosByUserIdQueryHandler : IRequestHandler<GetUserPortfoliosByUserIdQuery, IDataResult<IEnumerable<UserPortfolio>>>
        {
            readonly IUserPortfolioRepository _userPortfolioRepository;

            public GetUserPortfoliosByUserIdQueryHandler(IUserPortfolioRepository userPortfolioRepository)
            {
                _userPortfolioRepository = userPortfolioRepository;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<UserPortfolio>>> Handle(GetUserPortfoliosByUserIdQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<UserPortfolio>>(await _userPortfolioRepository.GetListAsync(w => w.UserId == request.UserId));
            }
        }
    }
}