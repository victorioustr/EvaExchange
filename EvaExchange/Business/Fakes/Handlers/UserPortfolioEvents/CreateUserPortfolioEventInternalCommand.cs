using Business.Constants;
using Business.Fakes.Handlers.UserPortfolios;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Fakes.Handlers.UserPortfolioEvents
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateUserPortfolioEventInternalCommand : IRequest<IResult>
    {
        public Guid UserPortfolioId { get; set; }
        public UserPortfolioEventType UserPortfolioEventType { get; set; }
        public int Lot { get; set; }
        public decimal Rate { get; set; }

        public class CreateUserPortfolioEventInternalCommandHandler : IRequestHandler<CreateUserPortfolioEventInternalCommand, IResult>
        {
            readonly IUserPortfolioEventRepository _userPortfolioEventRepository;
            readonly IMediator _mediator;
            public CreateUserPortfolioEventInternalCommandHandler(IUserPortfolioEventRepository userPortfolioEventRepository, IMediator mediator)
            {
                _mediator = mediator;
                _userPortfolioEventRepository = userPortfolioEventRepository;
            }

            public async Task<IResult> Handle(CreateUserPortfolioEventInternalCommand request, CancellationToken cancellationToken)
            {
                var portfolio = await _mediator.Send(new GetUserPortfolioInternalQuery { Id = request.UserPortfolioId });

                if (!portfolio.Success || (portfolio.Data == null))
                    return new ErrorResult(Messages.UserPortfolioNotExist);

                var addedUserPortfolioEvent = new UserPortfolioEvent
                {
                    Id = Guid.NewGuid(),
                    UserPortfolioId = request.UserPortfolioId,
                    UserPortfolioEventType = request.UserPortfolioEventType,
                    Lot = request.Lot,
                    Rate = request.Rate,
                };

                _userPortfolioEventRepository.Add(addedUserPortfolioEvent);
                await _userPortfolioEventRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}