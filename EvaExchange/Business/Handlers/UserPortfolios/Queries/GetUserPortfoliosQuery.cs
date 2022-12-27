
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.UserPortfolios.Queries
{

    public class GetUserPortfoliosQuery : IRequest<IDataResult<IEnumerable<UserPortfolio>>>
    {
        public class GetUserPortfoliosQueryHandler : IRequestHandler<GetUserPortfoliosQuery, IDataResult<IEnumerable<UserPortfolio>>>
        {
            readonly IUserPortfolioRepository _userPortfolioRepository;

            public GetUserPortfoliosQueryHandler(IUserPortfolioRepository userPortfolioRepository)
            {
                _userPortfolioRepository = userPortfolioRepository;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<UserPortfolio>>> Handle(GetUserPortfoliosQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<UserPortfolio>>(await _userPortfolioRepository.GetListAsync());
            }
        }
    }
}