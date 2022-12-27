
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.ShareRateHistories.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteShareRateHistoryCommand : IRequest<IResult>
    {
        public Guid Id { get; set; }

        public class DeleteShareRateHistoryCommandHandler : IRequestHandler<DeleteShareRateHistoryCommand, IResult>
        {
            readonly IShareRateHistoryRepository _shareRateHistoryRepository;

            public DeleteShareRateHistoryCommandHandler(IShareRateHistoryRepository shareRateHistoryRepository)
            {
                _shareRateHistoryRepository = shareRateHistoryRepository;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteShareRateHistoryCommand request, CancellationToken cancellationToken)
            {
                var shareRateHistoryToDelete = await _shareRateHistoryRepository.GetAsync(p => p.Id == request.Id);

                if (shareRateHistoryToDelete == null)
                    return new ErrorResult(Messages.ShareRateHistoryNotExist);

                _shareRateHistoryRepository.Delete(shareRateHistoryToDelete);
                await _shareRateHistoryRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

