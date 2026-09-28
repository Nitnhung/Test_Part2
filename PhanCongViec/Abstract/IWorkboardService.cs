using PhanCongViec.DTOs;
using PhanCongViec.Models;

namespace PhanCongViec.Abstract
{
    public interface IWorkboardService
    {
        Task<bool> SoftDeletedAsync(long id);
        Task<DetailWorkItem> GetDetailWorkItemByIdAsync(long id);
        Task<WorkItemHistoryDto> GetHistoryDtoAsync(long id);
        Task<List<History>> GetHistoryByDateFormTo(DateTime? fromDate, DateTime? toDate);
        Task<DetailWorkItem> PostWorkItem(long id, string note);
    }
}
