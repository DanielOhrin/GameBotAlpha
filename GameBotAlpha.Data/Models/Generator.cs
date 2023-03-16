using System.ComponentModel.DataAnnotations;

namespace GameBotAlpha.Data.Models
{
    public class Generator
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int BuyPrice { get; set; }

        [Required]
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        [Required]
        public int Amount { get; set; }

        [Required]
        public int SecondsPerGeneration { get; set; }
    }
}
