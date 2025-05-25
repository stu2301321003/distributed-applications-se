using System.ComponentModel.DataAnnotations;

namespace VacationManager.Companies.Models
{
    public class CompanyCreateModel
    {
        [Required, StringLength(35, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
    }
}
