using Business.Constants;
using Business.Fakes.Handlers.Shares;
using Business.Fakes.Handlers.UserPortfolioEvents;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Fakes.Handlers.UserPortfolios
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateUserPortfolioInternalCommand : IRequest<IResult>
    {
        public int UserId { get; set; }
        public Guid ShareId { get; set; }
        public int Lot { get; set; }

        public class CreateUserPortfolioInternalCommandHandler : IRequestHandler<CreateUserPortfolioInternalCommand, IResult>
        {
            readonly IUserPortfolioRepository _userPortfolioRepository;
            readonly IShareRepository _shareRepository;
            readonly IMediator _mediator;
            public CreateUserPortfolioInternalCommandHandler(IUserPortfolioRepository userPortfolioRepository, IMediator mediator)
            {
                _userPortfolioRepository = userPortfolioRepository;
                _mediator = mediator;
            }

            public async Task<IResult> Handle(CreateUserPortfolioInternalCommand request, CancellationToken cancellationToken)
            {
                var isThereUserPortfolioRecord = _userPortfolioRepository.Query().Any(u => (u.UserId == request.UserId) && (u.ShareId == request.ShareId));

                if (isThereUserPortfolioRecord == true)
                    return new ErrorResult(Messages.UserPortfolioAlreadyExist);

                var share = await _mediator.Send(new GetShareInternalQuery { Id = request.ShareId });

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

                var addedUserPortfolioEvent = new CreateUserPortfolioEventInternalCommand
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