using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Features.Urls.Queries.ResolveShortUrl;

namespace Url_Shortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedirectController : ControllerBase
    {


        private readonly IMediator _mediator;
        public RedirectController(IMediator mediator) => _mediator = mediator;

        [HttpGet("/{shortCode}")]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = Request.Headers["User-Agent"].ToString();
            var referrer = Request.Headers["Referer"].ToString();


            var originalUrl = await _mediator.Send(
                new ResolveShortUrlQuery(shortCode, ip, userAgent, referrer)
            );


            return Redirect(originalUrl);
        }

    }
}
