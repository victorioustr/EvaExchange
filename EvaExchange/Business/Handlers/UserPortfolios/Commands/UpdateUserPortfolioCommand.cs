
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.UserPortfolios.ValidationRules;
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


namespace Business.Handlers.UserPortfolios.Commands
{


    public class UpdateUserPortfolioCommand : IRequest<IResult>
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public Guid ShareId { get; set; }

        public class UpdateUserPortfolioCommandHandler : IRequestHandler<UpdateUserPortfolioCommand, IResult>
        {
            readonly IUserPortfolioRepository _userPortfolioRepository;

            public UpdateUserPortfolioCommandHandler(IUserPortfolioRepository userPortfolioRepository)
            {
                _userPortfolioRepository = userPortfolioRepository;
            }

            [ValidationAspect(typeof(UpdateUserPortfolioValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateUserPortfolioCommand request, CancellationToken cancellationToken)
            {
                var isThereUserPortfolioRecord = await _userPortfolioRepository.GetAsync(u => u.Id == request.Id);

                if (isThereUserPortfolioRecord == null)
                    return new ErrorResult(Messages.UserPortfolioNotExist);

                isThereUserPortfolioRecord.UserId = request.UserId;
                isThereUserPortfolioRecord.ShareId = request.ShareId;

                _userPortfolioRepository.Update(isThereUserPortfolioRecord);
                await _userPortfolioRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

