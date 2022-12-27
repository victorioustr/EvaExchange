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


namespace Business.Handlers.Shares.Queries
{
    public class GetShareQuery : IRequest<IDataResult<Share>>
    {
        public Guid Id { get; set; }

        public class GetShareQueryHandler : IRequestHandler<GetShareQuery, IDataResult<Share>>
        {
            readonly IShareRepository _shareRepository;

            public GetShareQueryHandler(IShareRepository shareRepository)
            {
                _shareRepository = shareRepository;
            }

            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Share>> Handle(GetShareQuery request, CancellationToken cancellationToken)
            {
                var share = await _shareRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Share>(share);
            }
        }
    }
}
