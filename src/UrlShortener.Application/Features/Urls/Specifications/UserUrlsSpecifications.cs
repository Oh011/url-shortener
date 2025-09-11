using Project.Application.Features.Urls.Enums;
using Project.Application.Features.Urls.Queries.GetUserUrls;
using Project.Domain.Entities;
using Project.Domain.Specifications;

namespace Project.Application.Features.Urls.Specifications
{
    internal class UserUrlsSpecifications : BaseSpecifications<UserAnalytics>
    {


        public UserUrlsSpecifications(GetUserUrlsQuery query, string userId) : base(

            u => u.UserId == userId &&
            (string.IsNullOrEmpty(query.Search) || (u.ShortUrl.Contains(query.Search) ||
            u.OriginalUrl.Contains(query.Search)))

            )
        {




            switch (query.sortOptions)
            {
                case UrlsSortOptions.CreatedAtAsc:
                    SetOrderBy(u => u.CreatedAt);
                    break;
                case UrlsSortOptions.CreatedAtDesc:
                    SetOrderByDescending(u => u.CreatedAt);
                    break;

                case UrlsSortOptions.TotalClicksAsc:
                    SetOrderBy(u => u.ClickCount);
                    break;
                case UrlsSortOptions.TotalClicksDesc:
                    SetOrderByDescending(u => u.ClickCount);
                    break;

                case UrlsSortOptions.ExpirationDateAsc:
                    SetOrderBy(u => u.ExpiresAt);
                    break;
                case UrlsSortOptions.ExpirationDateDesc:
                    SetOrderByDescending(u => u.ExpiresAt);
                    break;

                default:
                    SetOrderByDescending(u => u.CreatedAt); // default: newest first
                    break;
            }

            // 📄 Pagination
            ApplyPagination(query.PageIndex, query.pageSize);



        }
    }
}
