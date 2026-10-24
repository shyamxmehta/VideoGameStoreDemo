using System.ComponentModel.DataAnnotations;

namespace AdoDotNetTutorial.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Game { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
