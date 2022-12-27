
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.Shares.Queries;
using Business.Handlers.UserPortfolioEvents.Commands;
using Business.Handlers.UserPortfolios.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.UserPortfolios.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateUserPortfolioCommand : IRequest<IResult>
    {
        public int UserId { get; set; }
        public Guid ShareId { get; set; }
        public int Lot { get; set; }

        public class CreateUserPortfolioCommandHandler : IRequestHandler<CreateUserPortfolioCommand, IResult>
        {
            readonly IUserPortfolioRepository _userPortfolioRepository;
            readonly IShareRepository _shareRepository;
            readonly IMediator _mediator;
            public CreateUserPortfolioCommandHandler(IUserPortfolioRepository userPortfolioRepository, IMediator mediator)
            {
                _userPortfolioRepository = userPortfolioRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateUserPortfolioValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateUserPortfolioCommand request, CancellationToken cancellationToken)
            {
                var isThereUserPortfolioRecord = _userPortfolioRepository.Query().Any(u => (u.UserId == request.UserId) && (u.ShareId == request.ShareId));

                if (isThereUserPortfolioRecord == true)
                    return new ErrorResult(Messages.UserPortfolioAlreadyExist);

                var share = await _mediator.Send(new GetShareQuery { Id = request.ShareId });

                if (!share.Success || (share.Data == null))
                    return new ErrorResult(Messages.ShareNotExist);

                var addedUserPortfolio = new UserPortfolio
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    ShareId = request.ShareId,
                    Lot = request.Lot
                };

                _userPortfolioRepository.Add(addedUserPortfolio);
                await _userPortfolioRepository.SaveChangesAsync();

                var addedUserPortfolioEvent = new CreateUserPortfolioEventCommand
                {
                    UserPortfolioId = addedUserPortfolio.Id,
                    Lot = addedUserPortfolio.Lot,
                    Rate = share.Data.Rate,
                    UserPortfolioEventType = UserPortfolioEventType.Buy
                };

                await _mediator.Send(addedUserPortfolioEvent);

                return new SuccessResult(Messages.Added);
            }
        }
    }
}