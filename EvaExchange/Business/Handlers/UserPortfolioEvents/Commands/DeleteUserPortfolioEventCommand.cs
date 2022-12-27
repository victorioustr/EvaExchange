
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


namespace Business.Handlers.UserPortfolioEvents.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteUserPortfolioEventCommand : IRequest<IResult>
    {
        public Guid Id { get; set; }

        public class DeleteUserPortfolioEventCommandHandler : IRequestHandler<DeleteUserPortfolioEventCommand, IResult>
        {
            readonly IUserPortfolioEventRepository _userPortfolioEventRepository;

            public DeleteUserPortfolioEventCommandHandler(IUserPortfolioEventRepository userPortfolioEventRepository)
            {
                _userPortfolioEventRepository = userPortfolioEventRepository;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteUserPortfolioEventCommand request, CancellationToken cancellationToken)
            {
                var userPortfolioEventToDelete = await _userPortfolioEventRepository.GetAsync(p => p.Id == request.Id);

                if (userPortfolioEventToDelete == null)
                    return new ErrorResult(Messages.UserPortfolioEventNotExist);

                _userPortfolioEventRepository.Delete(userPortfolioEventToDelete);
                await _userPortfolioEventRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

