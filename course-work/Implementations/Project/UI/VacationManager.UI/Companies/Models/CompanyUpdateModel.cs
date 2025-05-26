using System.ComponentModel.DataAnnotations;

namespace VacationManager.UI.Companies.Models
{
    public class CompanyUpdateModel
    {
        [Required]
        public int Id { get; set; }

        [Required, StringLength(35, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
    }
}
