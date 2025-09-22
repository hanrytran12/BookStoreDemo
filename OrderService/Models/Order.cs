namespace OrderService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateOnly OrderDate { get; set; }
    }
}
