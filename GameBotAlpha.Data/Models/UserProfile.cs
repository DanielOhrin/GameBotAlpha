using System.ComponentModel.DataAnnotations;

namespace GameBotAlpha.Data.Models
{
    public class UserProfile
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string DiscordUid { get; set; } = null!;

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public int Balance { get; set; }

        [Required]
        public int GeneratorId { get; set; }
        public Generator Generator { get; set; } = null!;

        [Required]
        public int BackpackId { get; set; }
        public Backpack Backpack { get; set; } = null!;

        [Required]
        public DateTime LastSell { get; set; }
    }
}
