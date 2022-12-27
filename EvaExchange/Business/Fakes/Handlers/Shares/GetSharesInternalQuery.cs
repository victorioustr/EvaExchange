using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Fakes.Handlers.Shares
{

    public class GetSharesInternalQuery : IRequest<IDataResult<IEnumerable<Share>>>
    {
        public class GetSharesInternalQueryHandler : IRequestHandler<GetSharesInternalQuery, IDataResult<IEnumerable<Share>>>
        {
            readonly IShareRepository _shareRepository;

            public GetSharesInternalQueryHandler(IShareRepository shareRepository)
            {
                _shareRepository = shareRepository;
            }

            public async Task<IDataResult<IEnumerable<Share>>> Handle(GetSharesInternalQuery request, CancellationToken cancellationToken)
            {
                var shareList = await _shareRepository.GetListAsync();
                return new SuccessDataResult<IEnumerable<Share>>(shareList);
            }
        }
    }
}