using Business.Constants;
using Business.Handlers.Authorizations.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Fakes.Handlers.Authorizations
{
    public class RegisterUserInternalCommand : IRequest<IDataResult<User>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }


        public class RegisterUserInternalCommandHandler : IRequestHandler<RegisterUserInternalCommand, IDataResult<User>>
        {
            readonly IUserRepository _userRepository;


            public RegisterUserInternalCommandHandler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }


            [ValidationAspect(typeof(RegisterUserValidator), Priority = 2)]
            [CacheRemoveAspect()]
            public async Task<IDataResult<User>> Handle(RegisterUserInternalCommand request, CancellationToken cancellationToken)
            {
                var isThereAnyUser = await _userRepository.GetAsync(u => u.Email == request.Email);

                if (isThereAnyUser != null)
                    return new ErrorDataResult<User>(Messages.NameAlreadyExist);

                HashingHelper.CreatePasswordHash(request.Password, out var passwordSalt, out var passwordHash);
                var user = new User
                {
                    Email = request.Email,

                    FullName = request.FullName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true
                };

                _userRepository.Add(user);
                await _userRepository.SaveChangesAsync();
                return new SuccessDataResult<User>(user, Messages.Added);
            }
        }
    }
}