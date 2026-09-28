namespace PhanCongViec.DTOs
{
    public class ReportProjectByDay : ProjectDto
    {
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int TotalItem {  get; set; }
        public int OpentItem {  get; set; }
        public int OverDeuItem {  get; set; }
        public int Doneitem {  get; set; }
        public decimal AvarageCompletion {  get; set; }
    }
}
