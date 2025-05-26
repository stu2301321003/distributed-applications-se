using System.ComponentModel.DataAnnotations;

namespace VacationManager.UI.Companies.Models
{
    public class CompanyCreateModel
    {
        [Required, StringLength(35, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int CeoId{ get; set; }
    }
}
