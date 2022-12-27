using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Fakes.Handlers.ShareRateHistories
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateShareRateHistoryInternalCommand : IRequest<IResult>
    {
        public Guid ShareId { get; set; }
        public decimal Rate { get; set; }
        public DateTime UpdatedDate { get; set; }


        public class CreateShareRateHistoryInternalCommandHandler : IRequestHandler<CreateShareRateHistoryInternalCommand, IResult>
        {
            readonly IShareRateHistoryRepository _shareRateHistoryRepository;
            public CreateShareRateHistoryInternalCommandHandler(IShareRateHistoryRepository shareRateHistoryRepository)
            {
                _shareRateHistoryRepository = shareRateHistoryRepository;
            }

            public async Task<IResult> Handle(CreateShareRateHistoryInternalCommand request, CancellationToken cancellationToken)
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