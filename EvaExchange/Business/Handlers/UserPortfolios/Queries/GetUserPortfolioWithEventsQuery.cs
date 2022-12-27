
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.UserPortfolios.Queries
{
    public class GetUserPortfolioWithEventsQuery : IRequest<IDataResult<UserPortfolio>>
    {
        public Guid Id { get; set; }

        public class GetUserPortfolioWithEventsQueryHandler : IRequestHandler<GetUserPortfolioWithEventsQuery, IDataResult<UserPortfolio>>
        {
            readonly IUserPortfolioRepository _userPortfolioRepository;

            public GetUserPortfolioWithEventsQueryHandler(IUserPortfolioRepository userPortfolioRepository)
            {
                _userPortfolioRepository = userPortfolioRepository;
            }

            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<UserPortfolio>> Handle(GetUserPortfolioWithEventsQuery request, CancellationToken cancellationToken)
            {
                var userPortfolio = await _userPortfolioRepository.Query()
                    .Include(i => i.UserPortfolioEvents)
                    .Where(w => w.Id == request.Id)
                    .FirstOrDefaultAsync();

                return new SuccessDataResult<UserPortfolio>(userPortfolio);
            }
        }
    }
}
