using Business.BusinessAspects;
using Business.Fakes.Handlers.Authorizations;
using Business.Fakes.Handlers.OperationClaims;
using Business.Fakes.Handlers.Shares;
using Business.Fakes.Handlers.UserClaims;
using Business.Fakes.Handlers.UserPortfolios;
using Core.Utilities.IoC;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Business.Helpers
{
    public static class OperationClaimCreatorMiddleware
    {
        public static async Task UseDbOperationClaimCreator(this IApplicationBuilder app)
        {
            var mediator = ServiceTool.ServiceProvider.GetService<IMediator>();
            foreach (var operationName in GetOperationNames())
            {
                await mediator.Send(new CreateOperationClaimInternalCommand
                {
                    ClaimName = operationName
                });
            }

            await mediator.Send(new CreateOperationClaimInternalCommand
            {
                ClaimName = "BuyShellShareForUserCommand"
            });

            var operationClaims = (await mediator.Send(new GetOperationClaimsInternalQuery())).Data;
            var admin = await mediator.Send(new RegisterUserInternalCommand
            {
                FullName = "System Admin",
                Password = "P@ssw0rd",
                Email = "admin@eva.guru",
            });

            if (admin.Success)
            {
                await mediator.Send(
                    new CreateUserClaimsInternalCommand
                    {
                        UserId = admin.Data.UserId,
                        OperationClaims = operationClaims
                    });
            }

            var member = await mediator.Send(new RegisterUserInternalCommand
            {
                FullName = "Member",
                Password = "P@ssw0rd",
                Email = "member@eva.guru",
            });

            if (member.Success)
            {
                string[] claims = { "GetShareQuery", "BuyOwnShellShareCommand", "GetOwnUserPortfolioByShareIdQuery", "CreateOwnUserPortfolioEventCommand", "RecalculateUserPortfolioBalanceCommand" };

                await mediator.Send(
                    new CreateUserClaimsInternalCommand
                    {
                        UserId = member.Data.UserId,
                        OperationClaims = operationClaims.Where(w => claims.Contains(w.Name))
                    });
            }

            var shares = await mediator.Send(new GetSharesInternalQuery());

            if (shares.Data.Count() == 0)
            {
                await mediator.Send(new CreateShareInternalCommand { Code = "BTP", Name = "BilgiTAP", Rate = 1568.45m });
                await mediator.Send(new CreateShareInternalCommand { Code = "TAI", Name = "TAI", Rate = 1348.25m });
            }

            var userPortfolios = await mediator.Send(new GetUserPortfoliosInternalQuery());

            if (userPortfolios.Data.Count() == 0)
            {
                shares = await mediator.Send(new GetSharesInternalQuery());

                foreach (var share in shares.Data)
                {
                    await mediator.Send(
                        new CreateUserPortfolioInternalCommand { UserId = admin.Data.UserId, Lot = 0, ShareId = share.Id });
                    await mediator.Send(
                        new CreateUserPortfolioInternalCommand { UserId = member.Data.UserId, Lot = 0, ShareId = share.Id });
                }
            }
        }

        static IEnumerable<string> GetOperationNames()
        {
            var assemblies = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x =>
                    // runtime generated anonmous type'larin assemblysi olmadigi icin null cek yap
                    (x.Namespace != null) && x.Namespace.StartsWith("Business.Handlers") &&
                    (x.Name.EndsWith("Command") || x.Name.EndsWith("Query")));

            return (from assembly in assemblies
                    from nestedType in assembly.GetNestedTypes()
                    from method in nestedType.GetMethods()
                    where method.CustomAttributes.Any(u => u.AttributeType == typeof(SecuredOperation))
                    select assembly.Name).ToList();
        }
    }
}
