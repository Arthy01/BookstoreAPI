namespace BookstoreAPI.Models
{
    public class Order
    {
        public ulong Id { get; set; }
        public DateTime Issued { get; set; }
        public float Amount { get; set; }
        public ulong CustomerID { get; set; }
    }
}
