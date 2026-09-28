namespace PhanCongViec.DTOs
{
    public class SuccessRespone<T> : BaseRespone
    {
        public string TraceId { get; set; }
        public int Status {  get; set; }
        public string Message {  get; set; }
        public T? Data {  get; set; }
    }
}
