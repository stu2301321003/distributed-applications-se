using System.ComponentModel.DataAnnotations;

namespace VacationManager.UI.Teams.Models
{
    public class TeamCreateModel
    {
        [Required, StringLength(35, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int ManagerId { get; set; }
    }
}
