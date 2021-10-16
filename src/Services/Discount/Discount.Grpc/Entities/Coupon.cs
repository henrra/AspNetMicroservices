namespace Discount.Grpc.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }

        public override string ToString()
        {
            return $"Id : {Id}, ProductName : {ProductName}, Description: {Description}, Amount: {Amount}";
        }
    }
}