namespace InventoryService.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public DateOnly LastUpdated { get; set; }
    }
}
