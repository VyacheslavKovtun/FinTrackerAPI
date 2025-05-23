namespace FinTrackerAPI.Services.Interfaces.Models
{
    public enum ResponseResultCode
    {
        Success = 1,
        Failed = 2,
        NothingFound = 3
    }

    public class ResponseResult<T> where T : class
    {
        public ResponseResultCode Code { get; set; }
        public string? Message { get; set; }
        public T? Value { get; set; }
        public IEnumerable<T>? Values { get; set; }
    }
}
