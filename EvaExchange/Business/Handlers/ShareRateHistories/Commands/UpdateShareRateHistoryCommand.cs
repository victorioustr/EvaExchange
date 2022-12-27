
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.ShareRateHistories.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.ShareRateHistories.Commands
{


    public class UpdateShareRateHistoryCommand : IRequest<IResult>
    {
        public Guid Id { get; set; }
        public Guid ShareId { get; set; }
        public decimal Rate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public class UpdateShareRateHistoryCommandHandler : IRequestHandler<UpdateShareRateHistoryCommand, IResult>
        {
            readonly IShareRateHistoryRepository _shareRateHistoryRepository;

            public UpdateShareRateHistoryCommandHandler(IShareRateHistoryRepository shareRateHistoryRepository)
            {
                _shareRateHistoryRepository = shareRateHistoryRepository;
            }

            [ValidationAspect(typeof(UpdateShareRateHistoryValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateShareRateHistoryCommand request, CancellationToken cancellationToken)
            {
                var isThereShareRateHistoryRecord = await _shareRateHistoryRepository.GetAsync(u => u.Id == request.Id);

                if (isThereShareRateHistoryRecord == null)
                    return new ErrorResult(Messages.ShareRateHistoryNotExist);

                isThereShareRateHistoryRecord.ShareId = request.ShareId;
                isThereShareRateHistoryRecord.Rate = request.Rate;
                isThereShareRateHistoryRecord.UpdatedDate = request.UpdatedDate;

                _shareRateHistoryRepository.Update(isThereShareRateHistoryRecord);
                await _shareRateHistoryRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

