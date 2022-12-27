using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Fakes.Handlers.UserPortfolios
{
    public class GetUserPortfolioInternalQuery : IRequest<IDataResult<UserPortfolio>>
    {
        public Guid Id { get; set; }

        public class GetUserPortfolioInternalQueryHandler : IRequestHandler<GetUserPortfolioInternalQuery, IDataResult<UserPortfolio>>
        {
            readonly IUserPortfolioRepository _userPortfolioRepository;

            public GetUserPortfolioInternalQueryHandler(IUserPortfolioRepository userPortfolioRepository)
            {
                _userPortfolioRepository = userPortfolioRepository;
            }

            public async Task<IDataResult<UserPortfolio>> Handle(GetUserPortfolioInternalQuery request, CancellationToken cancellationToken)
            {
                var userPortfolio = await _userPortfolioRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<UserPortfolio>(userPortfolio);
            }
        }
    }
}
