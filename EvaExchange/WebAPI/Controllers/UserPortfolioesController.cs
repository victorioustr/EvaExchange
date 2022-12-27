using Business.Handlers.UserPortfolios.Commands;
using Business.Handlers.UserPortfolios.Queries;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    /// <summary>
    /// UserPortfolios If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserPortfoliosController : BaseApiController
    {
        ///<summary>
        ///List UserPortfolios
        ///</summary>
        ///<remarks>UserPortfolios</remarks>
        ///<return>List UserPortfolios</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserPortfolioDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new GetUserPortfoliosQuery());
            if (result.Success)
                return Ok(Mapper.Map<IEnumerable<UserPortfolioDto>>(result.Data));
            return BadRequest(result.Message);
        }

        ///<summary>
        ///List UserPortfolios by UserId
        ///</summary>
        ///<remarks>UserPortfolios</remarks>
        ///<return>List UserPortfolios</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserPortfolioDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getallbyuserid")]
        public async Task<IActionResult> GetListByUserId(int userId)
        {
            var result = await Mediator.Send(new GetUserPortfoliosByUserIdQuery { UserId = userId });
            if (result.Success)
                return Ok(Mapper.Map<IEnumerable<UserPortfolioDto>>(result.Data));
            return BadRequest(result.Message);
        }

        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>UserPortfolios</remarks>
        ///<return>UserPortfolios List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserPortfolioDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetUserPortfolioQuery { Id = id });
            if (result.Success)
                return Ok(Mapper.Map<UserPortfolioDto>(result.Data));
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add UserPortfolio.
        /// </summary>
        /// <param name="createUserPortfolio"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserPortfolioCommand createUserPortfolio)
        {
            var result = await Mediator.Send(createUserPortfolio);
            if (result.Success)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update UserPortfolio.
        /// </summary>
        /// <param name="updateUserPortfolio"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserPortfolioCommand updateUserPortfolio)
        {
            var result = await Mediator.Send(updateUserPortfolio);
            if (result.Success)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete UserPortfolio.
        /// </summary>
        /// <param name="deleteUserPortfolio"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteUserPortfolioCommand deleteUserPortfolio)
        {
            var result = await Mediator.Send(deleteUserPortfolio);
            if (result.Success)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Buy/Sell Share Lot
        /// </summary>
        /// <param name="buySellShare"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("buysellshare")]
        public async Task<IActionResult> BuySellShare([FromBody] BuySellShareCommand buySellShare)
        {
            Int32.TryParse(HttpContext.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value, out int userId);

            var result = (buySellShare.UserId == userId) ? (await Mediator.Send(Mapper.Map<BuyOwnShellShareCommand>(buySellShare))) : (await Mediator.Send(buySellShare));

            if (result.Success)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }
    }
}
