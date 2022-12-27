using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.ShareRateHistories.Queries
{
    public class GetLastShareRateHistoryByShareIdQuery : IRequest<IDataResult<ShareRateHistory>>
    {
        public Guid Id { get; set; }

        public class GetLastShareRateHistoryByShareIdQueryHandler : IRequestHandler<GetLastShareRateHistoryByShareIdQuery, IDataResult<ShareRateHistory>>
        {
            readonly IShareRateHistoryRepository _shareRateHistoryRepository;

            public GetLastShareRateHistoryByShareIdQueryHandler(IShareRateHistoryRepository shareRateHistoryRepository)
            {
                _shareRateHistoryRepository = shareRateHistoryRepository;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<ShareRateHistory>> Handle(GetLastShareRateHistoryByShareIdQuery request, CancellationToken cancellationToken)
            {
                var shareRateHistory = await _shareRateHistoryRepository.GetLastShareRateHistoryByShareIdAsync(request.Id);
                return new SuccessDataResult<ShareRateHistory>(shareRateHistory);
            }
        }
    }
}
