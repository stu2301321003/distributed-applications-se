namespace VacationManager.Companies.Models
{
    public class CompanyReadModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CeoId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
