using Business.Constants;
using Business.Fakes.Handlers.ShareRateHistories;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Fakes.Handlers.Shares
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateShareInternalCommand : IRequest<IResult>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }

        public class CreateShareInternalCommandHandler : IRequestHandler<CreateShareInternalCommand, IResult>
        {
            readonly IShareRepository _shareRepository;
            readonly IMediator _mediator;
            public CreateShareInternalCommandHandler(IShareRepository shareRepository, IMediator mediator)
            {
                _shareRepository = shareRepository;
                _mediator = mediator;
            }

            public async Task<IResult> Handle(CreateShareInternalCommand request, CancellationToken cancellationToken)
            {
                var isThereShareRecord = _shareRepository.Query().Any(u => u.Code == request.Code);

                if (isThereShareRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedShare = new Share
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Code = request.Code,
                    Rate = request.Rate,
                };

                _shareRepository.Add(addedShare);

                await _shareRepository.SaveChangesAsync();

                await _mediator.Send(
                    new CreateShareRateHistoryInternalCommand
                    {
                        ShareId = addedShare.Id,
                        Rate = request.Rate,
                        UpdatedDate = DateTime.UtcNow
                    });

                return new SuccessResult(Messages.Added);
            }
        }
    }
}