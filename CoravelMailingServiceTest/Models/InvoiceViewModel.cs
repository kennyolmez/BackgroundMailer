namespace CoravelMailingServiceTest.Models
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public string Recipient { get; set; }
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();

        public decimal TotalOrderPrice()
        {
            decimal total = 0;

            foreach (var item in Products)
            {
                total += item.UnitPrice * item.Quantity;
            }
            return total;
        }
    }
}
