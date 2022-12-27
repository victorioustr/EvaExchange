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

namespace Business.Handlers.ShareRateHistories.Queries
{

    public class GetShareRateHistoriesByShareIdQuery : IRequest<IDataResult<IEnumerable<ShareRateHistory>>>
    {
        public Guid Id { get; set; }

        public class GetShareRateHistoriesQueryHandler : IRequestHandler<GetShareRateHistoriesByShareIdQuery, IDataResult<IEnumerable<ShareRateHistory>>>
        {
            readonly IShareRateHistoryRepository _shareRateHistoryRepository;

            public GetShareRateHistoriesQueryHandler(IShareRateHistoryRepository shareRateHistoryRepository)
            {
                _shareRateHistoryRepository = shareRateHistoryRepository;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<ShareRateHistory>>> Handle(GetShareRateHistoriesByShareIdQuery request, CancellationToken cancellationToken)
            {
                var shareRateHistoryList = await _shareRateHistoryRepository.GetListAsync(w => w.ShareId == request.Id);
                return new SuccessDataResult<IEnumerable<ShareRateHistory>>(shareRateHistoryList);
            }
        }
    }
}