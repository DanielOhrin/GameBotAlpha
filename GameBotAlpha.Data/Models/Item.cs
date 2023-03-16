using System.ComponentModel.DataAnnotations;

namespace GameBotAlpha.Data.Models
{
    public class Item
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
        
        [Required]
        public int SellPrice { get; set; }
    }
}
