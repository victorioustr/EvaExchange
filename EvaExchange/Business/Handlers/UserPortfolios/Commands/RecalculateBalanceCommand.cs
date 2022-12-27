using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.UserPortfolios.Commands
{
    public class RecalculateUserPortfolioBalanceCommand : IRequest<IResult>
    {
        public Guid UserPortfolioId { get; set; }

        public class RecalculateUserPortfolioBalanceCommandHandler : IRequestHandler<RecalculateUserPortfolioBalanceCommand, IResult>
        {
            readonly IUserPortfolioRepository _userPortfolioRepository;
            readonly IMediator _mediator;

            public RecalculateUserPortfolioBalanceCommandHandler(IUserPortfolioRepository userPortfolioRepository, IMediator mediator)
            {
                _userPortfolioRepository = userPortfolioRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(RecalculateUserPortfolioBalanceCommand request, CancellationToken cancellationToken)
            {
                var portfolio = await _userPortfolioRepository.Query()
                    .Include(i => i.UserPortfolioEvents)
                    .Where(w => w.Id == request.UserPortfolioId)
                    .FirstOrDefaultAsync();

                if (portfolio == null)
                    return new ErrorResult(Messages.UserPortfolioNotExist);

                portfolio.Lot = portfolio.UserPortfolioEvents
                    .Where(w => w.UserPortfolioEventType == UserPortfolioEventType.Buy)
                    .Sum(s => s.Lot);

                portfolio.Lot -= portfolio.UserPortfolioEvents
                        .Where(w => w.UserPortfolioEventType == UserPortfolioEventType.Sell)
                        .Sum(s => s.Lot);

                _userPortfolioRepository.Update(portfolio);
                await _userPortfolioRepository.SaveChangesAsync();

                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

