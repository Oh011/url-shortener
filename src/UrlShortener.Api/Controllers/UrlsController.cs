using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Features.Urls.Commands.CreateShortUrl;
using Project.Application.Features.Urls.Commands.DeleteUrl;
using Project.Application.Features.Urls.Commands.UpdateUrl;
using Project.Application.Features.Urls.Dtos;
using Project.Application.Features.Urls.Queries.GetUrlClicksCount;
using Project.Application.Features.Urls.Queries.GetUrlDetails;
using Project.Application.Features.Urls.Queries.GetUserUrls;
using Shared;
using Shared.Dtos;
using Url_Shortner.Dtos.Urls;

namespace Url_Shortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController(IMediator mediator) : ControllerBase
    {


        [Authorize]
        [HttpGet("mine")]
        public async Task<ActionResult<SuccessWithData<PaginatedResult<UrlDetailsDto>>>> GetMyUrls(
    [FromQuery] GetUserUrlsQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(ApiResponseFactory.Success(result));
        }


        [HttpPost("shorten")]



        public async Task<SuccessWithData<ShortUrlDto>> CreateShortUrl([FromBody] CreateShortUrlCommand command)
        {


            var result = await mediator.Send(command);


            return ApiResponseFactory.Success<ShortUrlDto>(result);


        }


        [HttpGet("{shortUrl}/clicks")]

        public async Task<SuccessWithData<UrlClickCountsDto>> GetUrlClickCount(string shortUrl)
        {

            var result = await mediator.Send(new GetUrlClicksCountQuery(shortUrl));


            return ApiResponseFactory.Success<UrlClickCountsDto>(result);
        }


        [Authorize]
        [HttpPatch("{shortCode}")]
        public async Task<IActionResult> UpdateUrl(string shortCode, UpdateUrlDto dto)
        {
            var updatedUrl = await mediator.Send(new UpdateUrlCommand
            {
                ShortUrl = shortCode,
                NewExpiresAt = dto.NewExpiresAt
            });

            return Ok(updatedUrl);
        }

        [Authorize]
        [HttpDelete("{shortUrl}")]

        public async Task<IActionResult> DeleteUrl(string shortUrl)
        {

            await mediator.Send(new DeleteUrlCommand(shortUrl));


            return NoContent();
        }

        [HttpGet("{shortUrl}")]
        public async Task<SuccessWithData<UrlDetailsDto>> GetUrlInfo(string shortUrl)
        {
            var result = await mediator.Send(new GetUrlDetailsQuery(shortUrl));
            return ApiResponseFactory.Success(result);
        }
    }
}
