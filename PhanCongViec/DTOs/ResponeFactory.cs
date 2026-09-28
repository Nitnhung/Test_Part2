namespace PhanCongViec.DTOs
{
    public class ResponeFactory
    {
        public static SuccessRespone<T> Success <T> (string traceId, T data, string message = "success")
        {
            return new SuccessRespone<T>
            {
                TraceId = traceId,
                Status = 200,
                Message = message,
                Data = data
            };
        }
        public static FailRespone Fail (string traceId, int status, string message, Dictionary<string, string[]>? error = null)
        {
            return new FailRespone
            {
                TraceId = traceId,
                Status = status,
                Message = message,
                Error = error

            };
        }
            
    }
}
