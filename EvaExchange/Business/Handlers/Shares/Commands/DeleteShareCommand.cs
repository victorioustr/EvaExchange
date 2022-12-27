
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


namespace Business.Handlers.Shares.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteShareCommand : IRequest<IResult>
    {
        public Guid Id { get; set; }

        public class DeleteShareCommandHandler : IRequestHandler<DeleteShareCommand, IResult>
        {
            readonly IShareRepository _shareRepository;

            public DeleteShareCommandHandler(IShareRepository shareRepository)
            {
                _shareRepository = shareRepository;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteShareCommand request, CancellationToken cancellationToken)
            {
                var shareToDelete = await _shareRepository.GetAsync(p => p.Id == request.Id);

                if (shareToDelete == null)
                    return new ErrorResult(Messages.ShareNotExist);

                _shareRepository.Delete(shareToDelete);
                await _shareRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

