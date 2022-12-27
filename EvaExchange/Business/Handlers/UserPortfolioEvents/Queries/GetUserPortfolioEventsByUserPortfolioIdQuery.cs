
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

namespace Business.Handlers.UserPortfolioEvents.Queries
{

    public class GetUserPortfolioEventsByUserPortfolioIdQuery : IRequest<IDataResult<IEnumerable<UserPortfolioEvent>>>
    {
        public Guid UserPortfolioId { get; set; }

        public class GetUserPortfolioEventsByUserPortfolioIdQueryHandler : IRequestHandler<GetUserPortfolioEventsByUserPortfolioIdQuery, IDataResult<IEnumerable<UserPortfolioEvent>>>
        {
            readonly IUserPortfolioEventRepository _userPortfolioEventRepository;

            public GetUserPortfolioEventsByUserPortfolioIdQueryHandler(IUserPortfolioEventRepository userPortfolioEventRepository)
            {
                _userPortfolioEventRepository = userPortfolioEventRepository;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<UserPortfolioEvent>>> Handle(GetUserPortfolioEventsByUserPortfolioIdQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<UserPortfolioEvent>>(await _userPortfolioEventRepository.GetListAsync(w => w.UserPortfolioId == request.UserPortfolioId));
            }
        }
    }
}