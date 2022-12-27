
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


namespace Business.Handlers.UserPortfolios.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteUserPortfolioCommand : IRequest<IResult>
    {
        public Guid Id { get; set; }

        public class DeleteUserPortfolioCommandHandler : IRequestHandler<DeleteUserPortfolioCommand, IResult>
        {
            readonly IUserPortfolioRepository _userPortfolioRepository;

            public DeleteUserPortfolioCommandHandler(IUserPortfolioRepository userPortfolioRepository)
            {
                _userPortfolioRepository = userPortfolioRepository;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteUserPortfolioCommand request, CancellationToken cancellationToken)
            {
                var userPortfolioToDelete = _userPortfolioRepository.Get(p => p.Id == request.Id);

                if (userPortfolioToDelete == null)
                    return new ErrorResult(Messages.UserPortfolioNotExist);

                _userPortfolioRepository.Delete(userPortfolioToDelete);
                await _userPortfolioRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

