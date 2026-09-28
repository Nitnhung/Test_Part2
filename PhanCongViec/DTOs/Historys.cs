namespace PhanCongViec.DTOs
{
    public class Historys
    {

        public DateTime CreatedAt { get; set; }
        public List<WorkItemHistoryDto> Infor { get; set; } = new List<WorkItemHistoryDto>();
    }
    public class HistoryItem
    {
         public long id { get; set; }
        public string? FromStatus { get; set; }
        public string ToStatus { get; set; } = string.Empty;
        public string? Note { get; set; }
        public string ChangedBy { get; set; }

    }
}

