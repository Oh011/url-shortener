using MediatR;
using UrlShortener.Application.Features.Urls.Dtos;
using UrlShortener.Application.Features.Urls.Enums;
using Shared.Dtos;
using Shared.Parameters;

namespace UrlShortener.Application.Features.Urls.Queries
{
    public class GetUserUrlsQuery : PaginationQueryParameters, IRequest<PaginatedResult<UrlDetailsDto>>
    {


        public string? Search { get; set; }
        public UrlsSortOptions sortOptions { get; set; }
    }
}
