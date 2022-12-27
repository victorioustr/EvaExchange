using Business.Handlers.ShareRateHistories.Queries;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    /// <summary>
    /// ShareRateHistories If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ShareRateHistoriesController : BaseApiController
    {
        ///<summary>
        ///List ShareRateHistories
        ///</summary>
        ///<remarks>ShareRateHistories</remarks>
        ///<return>List ShareRateHistories</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ShareRateHistoryDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new GetShareRateHistoriesQuery());
            if (result.Success)
                return Ok(Mapper.Map<IEnumerable<ShareRateHistoryDto>>(result.Data));
            return BadRequest(result.Message);
        }

        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>ShareRateHistories</remarks>
        ///<return>ShareRateHistories List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShareRateHistoryDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetShareRateHistoryQuery { Id = id });
            if (result.Success)
                return Ok(Mapper.Map<ShareRateHistoryDto>(result.Data));
            return BadRequest(result.Message);
        }

        ///<summary>
        ///It brings the details according to its share id.
        ///</summary>
        ///<remarks>ShareRateHistories</remarks>
        ///<return>ShareRateHistories List By ShareId</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ShareRateHistoryDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyshareid")]
        public async Task<IActionResult> GetByShareId(Guid id)
        {
            var result = await Mediator.Send(new GetShareRateHistoriesByShareIdQuery { Id = id });
            if (result.Success)
                return Ok(Mapper.Map<IEnumerable<ShareRateHistoryDto>>(result.Data));
            return BadRequest(result.Message);
        }

    }
}
