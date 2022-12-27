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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Shares.Queries
{

    public class GetSharesQuery : IRequest<IDataResult<IEnumerable<Share>>>
    {
        public class GetSharesQueryHandler : IRequestHandler<GetSharesQuery, IDataResult<IEnumerable<Share>>>
        {
            readonly IShareRepository _shareRepository;

            public GetSharesQueryHandler(IShareRepository shareRepository)
            {
                _shareRepository = shareRepository;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Share>>> Handle(GetSharesQuery request, CancellationToken cancellationToken)
            {
                var shareList = await _shareRepository.GetListAsync();
                return new SuccessDataResult<IEnumerable<Share>>(shareList);
            }
        }
    }
}