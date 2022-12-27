
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.ShareRateHistories.Commands;
using Business.Handlers.Shares.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Transaction;
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

namespace Business.Handlers.Shares.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateShareCommand : IRequest<IResult>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }

        public class CreateShareCommandHandler : IRequestHandler<CreateShareCommand, IResult>
        {
            readonly IShareRepository _shareRepository;
            readonly IMediator _mediator;
            public CreateShareCommandHandler(IShareRepository shareRepository, IMediator mediator)
            {
                _shareRepository = shareRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateShareValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            [TransactionScopeAspectAsync]
            public async Task<IResult> Handle(CreateShareCommand request, CancellationToken cancellationToken)
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
                    new CreateShareRateHistoryCommand
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