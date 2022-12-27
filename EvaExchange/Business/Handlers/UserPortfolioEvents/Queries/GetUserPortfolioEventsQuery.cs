
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

namespace Business.Handlers.UserPortfolioEvents.Queries
{

    public class GetUserPortfolioEventsQuery : IRequest<IDataResult<IEnumerable<UserPortfolioEvent>>>
    {
        public class GetUserPortfolioEventsQueryHandler : IRequestHandler<GetUserPortfolioEventsQuery, IDataResult<IEnumerable<UserPortfolioEvent>>>
        {
            readonly IUserPortfolioEventRepository _userPortfolioEventRepository;

            public GetUserPortfolioEventsQueryHandler(IUserPortfolioEventRepository userPortfolioEventRepository)
            {
                _userPortfolioEventRepository = userPortfolioEventRepository;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<UserPortfolioEvent>>> Handle(GetUserPortfolioEventsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<UserPortfolioEvent>>(await _userPortfolioEventRepository.GetListAsync());
            }
        }
    }
}