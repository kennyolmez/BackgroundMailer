namespace CoravelMailingServiceTest.Data.Entities
{
    public class MailProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string ImgUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public PendingMail Mail { get; set; }

    }
}
