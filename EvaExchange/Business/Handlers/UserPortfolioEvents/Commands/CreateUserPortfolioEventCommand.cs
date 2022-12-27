
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.UserPortfolioEvents.ValidationRules;
using Business.Handlers.UserPortfolios.Queries;
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

namespace Business.Handlers.UserPortfolioEvents.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateUserPortfolioEventCommand : IRequest<IResult>
    {
        public Guid UserPortfolioId { get; set; }
        public UserPortfolioEventType UserPortfolioEventType { get; set; }
        public int Lot { get; set; }
        public decimal Rate { get; set; }

        public class CreateUserPortfolioEventCommandHandler : IRequestHandler<CreateUserPortfolioEventCommand, IResult>
        {
            readonly IUserPortfolioEventRepository _userPortfolioEventRepository;
            readonly IMediator _mediator;
            public CreateUserPortfolioEventCommandHandler(IUserPortfolioEventRepository userPortfolioEventRepository, IMediator mediator)
            {
                _mediator = mediator;
                _userPortfolioEventRepository = userPortfolioEventRepository;
            }

            [ValidationAspect(typeof(CreateUserPortfolioEventValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateUserPortfolioEventCommand request, CancellationToken cancellationToken)
            {
                var portfolio = await _mediator.Send(new GetUserPortfolioQuery { Id = request.UserPortfolioId });

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