namespace GameBotAlpha.Data.Models
{
    public class Backpack
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int BuyPrice { get; set; }
        public int ItemLimit { get; set; }
    }
}
