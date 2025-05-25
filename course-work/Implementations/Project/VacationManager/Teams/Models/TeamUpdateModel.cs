using System.ComponentModel.DataAnnotations;

namespace VacationManager.Teams.Models
{
    public class TeamUpdateModel
    {
        [Required]
        public int Id { get; set; }

        [Required, StringLength(35, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int ManagerId { get; set; }
    }
}
