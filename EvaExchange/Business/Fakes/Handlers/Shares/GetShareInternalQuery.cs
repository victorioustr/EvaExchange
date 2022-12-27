using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Fakes.Handlers.Shares
{
    public class GetShareInternalQuery : IRequest<IDataResult<Share>>
    {
        public Guid Id { get; set; }

        public class GetShareInternalQueryHandler : IRequestHandler<GetShareInternalQuery, IDataResult<Share>>
        {
            readonly IShareRepository _shareRepository;

            public GetShareInternalQueryHandler(IShareRepository shareRepository)
            {
                _shareRepository = shareRepository;
            }

            public async Task<IDataResult<Share>> Handle(GetShareInternalQuery request, CancellationToken cancellationToken)
            {
                var share = await _shareRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Share>(share);
            }
        }
    }
}
