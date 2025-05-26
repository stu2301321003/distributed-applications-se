namespace VacationManager.UI.Commons.Models
{
    public class DataSourceRequest
    {
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
        public string? Filter { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; } = 10;
    }
}
