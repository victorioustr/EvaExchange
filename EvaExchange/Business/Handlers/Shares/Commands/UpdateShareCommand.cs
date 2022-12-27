
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.ShareRateHistories.Commands;
using Business.Handlers.ShareRateHistories.Queries;
using Business.Handlers.Shares.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.Shares.Commands
{


    public class UpdateShareCommand : IRequest<IResult>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }

        public class UpdateShareCommandHandler : IRequestHandler<UpdateShareCommand, IResult>
        {
            readonly IShareRepository _shareRepository;
            readonly IMediator _mediator;

            public UpdateShareCommandHandler(IShareRepository shareRepository, IMediator mediator)
            {
                _shareRepository = shareRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateShareValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            [TransactionScopeAspectAsync]
            public async Task<IResult> Handle(UpdateShareCommand request, CancellationToken cancellationToken)
            {
                var isThereShareRecord = await _shareRepository.GetAsync(u => u.Id == request.Id);

                if (isThereShareRecord == null)
                    return new ErrorResult(Messages.ShareNotExist);

                if (isThereShareRecord.Rate != request.Rate)
                {
                    // Rate Change Operations

                    var lastShareRateHistory = await _mediator.Send(new GetLastShareRateHistoryByShareIdQuery { Id = request.Id });
                    if ((lastShareRateHistory != null) && lastShareRateHistory.Success && (lastShareRateHistory.Data != null) && (lastShareRateHistory.Data.UpdatedDate > DateTime.UtcNow.AddHours(-1)))
                        return new ErrorResult(Messages.TimeIntervalException);

                    await _mediator.Send(
                        new CreateShareRateHistoryCommand
                        {
                            ShareId = request.Id,
                            Rate = request.Rate,
                            UpdatedDate = DateTime.UtcNow
                        });
                }

                isThereShareRecord.Name = request.Name;
                isThereShareRecord.Code = request.Code;
                isThereShareRecord.Rate = request.Rate;

                _shareRepository.Update(isThereShareRecord);
                await _shareRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

