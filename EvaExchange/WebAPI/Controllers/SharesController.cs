using Business.Handlers.Shares.Commands;
using Business.Handlers.Shares.Queries;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Shares If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SharesController : BaseApiController
    {
        ///<summary>
        ///List Shares
        ///</summary>
        ///<remarks>Shares</remarks>
        ///<return>List Shares</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ShareDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new GetSharesQuery());
            if (result.Success)
                return Ok(Mapper.Map<IEnumerable<ShareDto>>(result.Data));
            return BadRequest(result.Message);
        }

        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>Shares</remarks>
        ///<return>Shares List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShareDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetShareQuery { Id = id });
            if (result.Success)
                return Ok(Mapper.Map<ShareDto>(result.Data));
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add Share.
        /// </summary>
        /// <param name="createShare"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateShareCommand createShare)
        {
            var result = await Mediator.Send(createShare);
            if (result.Success)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update Share.
        /// </summary>
        /// <param name="updateShare"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateShareCommand updateShare)
        {
            var result = await Mediator.Send(updateShare);
            if (result.Success)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete Share.
        /// </summary>
        /// <param name="deleteShare"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteShareCommand deleteShare)
        {
            var result = await Mediator.Send(deleteShare);
            if (result.Success)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }
    }
}
