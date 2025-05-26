namespace VacationManager.UI.Api.Models
{
    public class ApiResult<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
