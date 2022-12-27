using AutoMapper;
using Business.Handlers.UserPortfolios.Commands;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Share, ShareDto>().ReverseMap();
            CreateMap<ShareRateHistory, ShareRateHistoryDto>().ReverseMap();
            CreateMap<UserPortfolio, UserPortfolioDto>().ReverseMap();
            CreateMap<UserPortfolioEvent, UserPortfolioEventDto>().ReverseMap();
            CreateMap<BuySellShareCommand, BuyOwnShellShareCommand>().ReverseMap();
        }
    }
}