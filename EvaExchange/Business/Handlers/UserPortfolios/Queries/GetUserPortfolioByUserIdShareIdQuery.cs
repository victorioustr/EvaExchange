
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
    public class GetUserPortfolioByUserIdShareIdQuery : IRequest<IDataResult<UserPortfolio>>
    {
        public int UserId { get; set; }
        public Guid ShareId { get; set; }

        public class GetUserPortfolioByUserIdShareIdQueryHandler : IRequestHandler<GetUserPortfolioByUserIdShareIdQuery, IDataResult<UserPortfolio>>
        {
            readonly IUserPortfolioRepository _userPortfolioRepository;

            public GetUserPortfolioByUserIdShareIdQueryHandler(IUserPortfolioRepository userPortfolioRepository)
            {
                _userPortfolioRepository = userPortfolioRepository;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<UserPortfolio>> Handle(GetUserPortfolioByUserIdShareIdQuery request, CancellationToken cancellationToken)
            {
                var userPortfolio = await _userPortfolioRepository.GetAsync(p => (p.UserId == request.UserId) && (p.ShareId == request.ShareId));
                return new SuccessDataResult<UserPortfolio>(userPortfolio);
            }
        }
    }
}
