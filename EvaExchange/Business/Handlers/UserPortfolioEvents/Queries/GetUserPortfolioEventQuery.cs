
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


namespace Business.Handlers.UserPortfolioEvents.Queries
{
    public class GetUserPortfolioEventQuery : IRequest<IDataResult<UserPortfolioEvent>>
    {
        public Guid Id { get; set; }

        public class GetUserPortfolioEventQueryHandler : IRequestHandler<GetUserPortfolioEventQuery, IDataResult<UserPortfolioEvent>>
        {
            readonly IUserPortfolioEventRepository _userPortfolioEventRepository;

            public GetUserPortfolioEventQueryHandler(IUserPortfolioEventRepository userPortfolioEventRepository)
            {
                _userPortfolioEventRepository = userPortfolioEventRepository;
            }

            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<UserPortfolioEvent>> Handle(GetUserPortfolioEventQuery request, CancellationToken cancellationToken)
            {
                var userPortfolioEvent = await _userPortfolioEventRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<UserPortfolioEvent>(userPortfolioEvent);
            }
        }
    }
}
