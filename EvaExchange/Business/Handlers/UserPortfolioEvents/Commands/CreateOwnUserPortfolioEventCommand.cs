
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.UserPortfolioEvents.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.UserPortfolioEvents.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateOwnUserPortfolioEventCommand : IRequest<IResult>
    {
        public Guid UserPortfolioId { get; set; }
        public UserPortfolioEventType UserPortfolioEventType { get; set; }
        public int Lot { get; set; }
        public decimal Rate { get; set; }

        public class CreateOwnUserPortfolioEventCommandHandler : IRequestHandler<CreateOwnUserPortfolioEventCommand, IResult>
        {
            readonly IHttpContextAccessor _contextAccessor;
            readonly IUserPortfolioEventRepository _userPortfolioEventRepository;
            readonly IUserPortfolioRepository _userPortfolioRepository;
            readonly IMediator _mediator;
            public CreateOwnUserPortfolioEventCommandHandler(IUserPortfolioEventRepository userPortfolioEventRepository, IUserPortfolioRepository userPortfolioRepository, IHttpContextAccessor contextAccessor, IMediator mediator)
            {
                _userPortfolioRepository = userPortfolioRepository;
                _contextAccessor = contextAccessor;
                _userPortfolioEventRepository = userPortfolioEventRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateUserPortfolioEventValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateOwnUserPortfolioEventCommand request, CancellationToken cancellationToken)
            {
                var userId = _contextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;

                if (userId == null)
                    throw new SecurityException(Messages.AuthorizationsDenied);

                var portfolio = _userPortfolioRepository.GetList(w => w.Id == request.UserPortfolioId).FirstOrDefault();

                if (portfolio == null)
                    return new ErrorResult(Messages.UserPortfolioNotExist);

                if (portfolio.UserId != Int32.Parse(userId))
                    throw new SecurityException(Messages.AuthorizationsDenied);

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