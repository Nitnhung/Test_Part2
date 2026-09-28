using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.NetworkInformation;

namespace PhanCongViec.DTOs
{
    public class WorkItemDto
    {

            public long Id { get; set; }
            public string Code { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string? Description { get; set; }
            public string Status { get; set; } = string.Empty;
            public string Priority { get; set; } = string.Empty;
            public DateTime? DueAt { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public DateTime? CompletedAt { get; set; }
        
    }
}
