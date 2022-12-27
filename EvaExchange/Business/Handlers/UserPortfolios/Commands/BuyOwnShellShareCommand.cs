using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.Shares.Queries;
using Business.Handlers.UserPortfolioEvents.Commands;
using Business.Handlers.UserPortfolios.Queries;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.UserPortfolios.Commands
{
    public class BuyOwnShellShareCommand : IRequest<IResult>
    {
        public int UserId { get; set; }
        public Guid ShareId { get; set; }
        public UserPortfolioEventType TradeType { get; set; }
        public int Lot { get; set; }

        public class BuyOwnShellShareCommandHandler : IRequestHandler<BuyOwnShellShareCommand, IResult>
        {
            readonly IMediator _mediator;
            readonly IHttpContextAccessor _contextAccessor;
            readonly ICacheManager _cacheManager;

            public BuyOwnShellShareCommandHandler(IMediator mediator, IHttpContextAccessor contextAccessor, ICacheManager cacheManager)
            {
                _cacheManager = cacheManager;
                _contextAccessor = contextAccessor;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(BuyOwnShellShareCommand request, CancellationToken cancellationToken)
            {
                var userId = _contextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;

                if (userId == null)
                    throw new SecurityException(Messages.AuthorizationsDenied);

                var oprClaims = _cacheManager.Get($"{CacheKeys.UserIdForClaim}={userId}") as IEnumerable<string>;

                if (!oprClaims.Contains("BuyShellShareCommand") && (!oprClaims.Contains("BuyOwnShellShareCommand") || (request.UserId != Int32.Parse(userId))))
                    throw new SecurityException(Messages.AuthorizationsDenied);

                var share = await _mediator.Send(new GetShareQuery { Id = request.ShareId });

                if (!share.Success || (share.Data == null))
                    return new ErrorResult(Messages.ShareNotExist);

                var portfolio = await _mediator.Send(new GetOwnUserPortfolioByShareIdQuery { ShareId = share.Data.Id });

                if (!portfolio.Success || (portfolio.Data == null))
                    return new ErrorResult(Messages.UserPortfolioNotExist);

                if ((request.TradeType == UserPortfolioEventType.Sell) && (portfolio.Data.Lot < request.Lot))
                    return new ErrorResult(Messages.InsufficientBalance);

                var userPortfolioEvent = new CreateOwnUserPortfolioEventCommand
                {
                    UserPortfolioId = portfolio.Data.Id,
                    UserPortfolioEventType = request.TradeType,
                    Lot = request.Lot,
                    Rate = share.Data.Rate
                };

                await _mediator.Send(userPortfolioEvent);

                await _mediator.Send(new RecalculateUserPortfolioBalanceCommand
                {
                    UserPortfolioId = portfolio.Data.Id
                });

                return new SuccessResult(Messages.Added);
            }
        }
    }
}

