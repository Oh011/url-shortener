using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Features.Analytics.Dtos;
using Project.Application.Features.Analytics.queries.TopUserUrlsQuery;
using Project.Application.Features.Analytics.queries.UserAnalyticsSummary;
using Shared;

namespace Url_Shortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController(IMediator mediator) : ControllerBase
    {


        [Authorize]
        [HttpGet("summary")]

        public async Task<ActionResult<SuccessWithData<UserAnalyticsSummaryDto>>> GetSummary()
        {
            var result = await mediator.Send(new GetUserAnalyticsSummaryQuery());

            return Ok(ApiResponseFactory.Success<UserAnalyticsSummaryDto>(result));
        }


        /// <summary>
        /// Returns top N URLs of the authenticated user, ordered by click count.
        /// </summary>

        [Authorize]
        [HttpGet("top-urls")]
        public async Task<ActionResult<SuccessWithData<List<TopUrlDto>>>> GetTopUrls([FromQuery] int count = 5)
        {



            var result = await mediator.Send(new GetTopUserUrlsQuery(count));

            return Ok(ApiResponseFactory.Success(result));
        }
    }
}
