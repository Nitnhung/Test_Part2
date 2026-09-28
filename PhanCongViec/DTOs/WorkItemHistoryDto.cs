using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;


namespace PhanCongViec.DTOs
{
    public class WorkItemHistoryDto
    {

        public long id { get; set; }
        public string? FromStatus { get; set; }
        public string ToStatus { get; set; } = string.Empty;
        [MaxLength(1001, ErrorMessage = "max 1000 ky tu")]
        public string? Note { get; set; }
        public string ChangedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class History
    {

        public DateTime CreatedAt { get; set; }
        public List<WorkItemHistoryDto> Infor { get; set; } = new List<WorkItemHistoryDto>();
    }

}
