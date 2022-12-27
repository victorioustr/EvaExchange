
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.UserPortfolioEvents.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.UserPortfolioEvents.Commands
{


    public class UpdateUserPortfolioEventCommand : IRequest<IResult>
    {
        public Guid Id { get; set; }
        public Guid UserPortfolioId { get; set; }
        public UserPortfolioEventType UserPortfolioEventType { get; set; }
        public int Lot { get; set; }
        public decimal Rate { get; set; }

        public class UpdateUserPortfolioEventCommandHandler : IRequestHandler<UpdateUserPortfolioEventCommand, IResult>
        {
            readonly IUserPortfolioEventRepository _userPortfolioEventRepository;
            readonly IMediator _mediator;

            public UpdateUserPortfolioEventCommandHandler(IUserPortfolioEventRepository userPortfolioEventRepository, IMediator mediator)
            {
                _userPortfolioEventRepository = userPortfolioEventRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateUserPortfolioEventValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateUserPortfolioEventCommand request, CancellationToken cancellationToken)
            {
                var isThereUserPortfolioEventRecord = await _userPortfolioEventRepository.GetAsync(u => u.Id == request.Id);

                if (isThereUserPortfolioEventRecord == null)
                    return new ErrorResult(Messages.UserPortfolioEventNotExist);

                isThereUserPortfolioEventRecord.UserPortfolioId = request.UserPortfolioId;
                isThereUserPortfolioEventRecord.UserPortfolioEventType = request.UserPortfolioEventType;
                isThereUserPortfolioEventRecord.Lot = request.Lot;
                isThereUserPortfolioEventRecord.Rate = request.Rate;

                _userPortfolioEventRepository.Update(isThereUserPortfolioEventRecord);
                await _userPortfolioEventRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

