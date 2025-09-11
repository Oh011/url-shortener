using Project.Application.Features.UrlAccessLogs.Dtos;

namespace Project.Application.Common.Interfaces.Services
{
    public interface IUrlAccessLogService
    {
        Task LogAccessAsync(UrlAccessLogDto logDto, CancellationToken cancellationToken = default);
    }




}
