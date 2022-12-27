
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.ShareRateHistories.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.ShareRateHistories.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateShareRateHistoryCommand : IRequest<IResult>
    {
        public Guid ShareId { get; set; }
        public decimal Rate { get; set; }
        public DateTime UpdatedDate { get; set; }


        public class CreateShareRateHistoryCommandHandler : IRequestHandler<CreateShareRateHistoryCommand, IResult>
        {
            readonly IShareRateHistoryRepository _shareRateHistoryRepository;
            public CreateShareRateHistoryCommandHandler(IShareRateHistoryRepository shareRateHistoryRepository)
            {
                _shareRateHistoryRepository = shareRateHistoryRepository;
            }

            [ValidationAspect(typeof(CreateShareRateHistoryValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateShareRateHistoryCommand request, CancellationToken cancellationToken)
            {

                var addedShareRateHistory = new ShareRateHistory
                {
                    Id = Guid.NewGuid(),
                    ShareId = request.ShareId,
                    Rate = request.Rate,
                    UpdatedDate = request.UpdatedDate,
                };

                _shareRateHistoryRepository.Add(addedShareRateHistory);
                await _shareRateHistoryRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}