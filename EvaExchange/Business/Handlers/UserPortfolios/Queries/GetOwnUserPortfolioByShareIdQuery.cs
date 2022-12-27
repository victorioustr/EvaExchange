
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.UserPortfolios.Queries
{
    public class GetOwnUserPortfolioByShareIdQuery : IRequest<IDataResult<UserPortfolio>>
    {
        public Guid ShareId { get; set; }

        public class GetOwnUserPortfolioByShareIdQueryHandler : IRequestHandler<GetOwnUserPortfolioByShareIdQuery, IDataResult<UserPortfolio>>
        {
            readonly IHttpContextAccessor _contextAccessor;
            readonly IUserPortfolioRepository _userPortfolioRepository;

            public GetOwnUserPortfolioByShareIdQueryHandler(IUserPortfolioRepository userPortfolioRepository, IHttpContextAccessor contextAccessor)
            {
                _contextAccessor = contextAccessor;
                _userPortfolioRepository = userPortfolioRepository;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<UserPortfolio>> Handle(GetOwnUserPortfolioByShareIdQuery request, CancellationToken cancellationToken)
            {
                var userId = _contextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;

                if (userId == null)
                    throw new SecurityException(Messages.AuthorizationsDenied);

                var userPortfolio = await _userPortfolioRepository.GetAsync(p => (p.UserId == Int32.Parse(userId)) && (p.ShareId == request.ShareId));
                return new SuccessDataResult<UserPortfolio>(userPortfolio);
            }
        }
    }
}
