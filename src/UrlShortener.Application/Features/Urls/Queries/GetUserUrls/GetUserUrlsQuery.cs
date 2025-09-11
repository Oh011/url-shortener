using MediatR;
using Project.Application.Features.Urls.Dtos;
using Project.Application.Features.Urls.Enums;
using Shared.Dtos;
using Shared.Parameters;

namespace Project.Application.Features.Urls.Queries.GetUserUrls
{
    public class GetUserUrlsQuery : PaginationQueryParameters, IRequest<PaginatedResult<UrlDetailsDto>>
    {


        public string? Search { get; set; }
        public UrlsSortOptions sortOptions { get; set; }
    }
}
