using PhanCongViec.Models;
using System.Reflection.Emit;

namespace PhanCongViec.DTOs
{
    public class DetailWorkItem
    {
        public WorkItemDto Item { get; set; } = null;
        public ProjectDto Project { get; set; } = null;
        public AssigneeDto? Assignee { get; set; }
        public List<string> Label { get; set; } = new();
        public List<WorkItemHistoryDto> WorkItemHistories { get; set; } = new();
    }
}
