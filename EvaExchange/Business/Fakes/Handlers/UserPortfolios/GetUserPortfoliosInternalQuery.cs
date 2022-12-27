using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Fakes.Handlers.UserPortfolios
{

    public class GetUserPortfoliosInternalQuery : IRequest<IDataResult<IEnumerable<UserPortfolio>>>
    {
        public class GetUserPortfoliosInternalQueryHandler : IRequestHandler<GetUserPortfoliosInternalQuery, IDataResult<IEnumerable<UserPortfolio>>>
        {
            readonly IUserPortfolioRepository _userPortfolioRepository;

            public GetUserPortfoliosInternalQueryHandler(IUserPortfolioRepository userPortfolioRepository)
            {
                _userPortfolioRepository = userPortfolioRepository;
            }

            public async Task<IDataResult<IEnumerable<UserPortfolio>>> Handle(GetUserPortfoliosInternalQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<UserPortfolio>>(await _userPortfolioRepository.GetListAsync());
            }
        }
    }
}