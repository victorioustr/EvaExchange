using Business.Handlers.UserPortfolioEvents.Commands;
using Business.Handlers.UserPortfolioEvents.Queries;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    /// <summary>
    /// UserPortfolioEvents If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserPortfolioEventsController : BaseApiController
    {
        ///<summary>
        ///List UserPortfolioEvents
        ///</summary>
        ///<remarks>UserPortfolioEvents</remarks>
        ///<return>List UserPortfolioEvents</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserPortfolioEventDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new GetUserPortfolioEventsQuery());
            if (result.Success)
                return Ok(Mapper.Map<IEnumerable<UserPortfolioEventDto>>(result.Data));
            return BadRequest(result.Message);
        }

        ///<summary>
        ///List UserPortfolioEvents by UserPortfolioId
        ///</summary>
        ///<remarks>UserPortfolioEvents</remarks>
        ///<return>List UserPortfolioEvents</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserPortfolioEventDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getallbyuserportfolioid")]
        public async Task<IActionResult> GetListByUserPortfolioId(Guid userPortfolioId)
        {
            var result = await Mediator.Send(new GetUserPortfolioEventsByUserPortfolioIdQuery { UserPortfolioId = userPortfolioId });

            if (result.Success)
                return Ok(Mapper.Map<IEnumerable<UserPortfolioEventDto>>(result.Data));
            return BadRequest(result.Message);
        }

        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>UserPortfolioEvents</remarks>
        ///<return>UserPortfolioEvents List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserPortfolioEventDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetUserPortfolioEventQuery { Id = id });
            if (result.Success)
                return Ok(Mapper.Map<UserPortfolioEventDto>(result.Data));
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add UserPortfolioEvent.
        /// </summary>
        /// <param name="createUserPortfolioEvent"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserPortfolioEventCommand createUserPortfolioEvent)
        {
            var result = await Mediator.Send(createUserPortfolioEvent);
            if (result.Success)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update UserPortfolioEvent.
        /// </summary>
        /// <param name="updateUserPortfolioEvent"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserPortfolioEventCommand updateUserPortfolioEvent)
        {
            var result = await Mediator.Send(updateUserPortfolioEvent);
            if (result.Success)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete UserPortfolioEvent.
        /// </summary>
        /// <param name="deleteUserPortfolioEvent"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteUserPortfolioEventCommand deleteUserPortfolioEvent)
        {
            var result = await Mediator.Send(deleteUserPortfolioEvent);
            if (result.Success)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }
    }
}
