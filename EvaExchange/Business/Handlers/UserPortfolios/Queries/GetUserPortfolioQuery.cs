
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.UserPortfolios.Queries
{
    public class GetUserPortfolioQuery : IRequest<IDataResult<UserPortfolio>>
    {
        public Guid Id { get; set; }

        public class GetUserPortfolioQueryHandler : IRequestHandler<GetUserPortfolioQuery, IDataResult<UserPortfolio>>
        {
            readonly IUserPortfolioRepository _userPortfolioRepository;

            public GetUserPortfolioQueryHandler(IUserPortfolioRepository userPortfolioRepository)
            {
                _userPortfolioRepository = userPortfolioRepository;
            }

            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<UserPortfolio>> Handle(GetUserPortfolioQuery request, CancellationToken cancellationToken)
            {
                var userPortfolio = await _userPortfolioRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<UserPortfolio>(userPortfolio);
            }
        }
    }
}
