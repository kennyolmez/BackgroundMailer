using CoravelMailingServiceTest.Data.Entities;
using CoravelMailingServiceTest.Models;

namespace CoravelMailingServiceTest.Services
{
    public class MailServices
    {
        public PendingMail MapInvoiceToPendingMail(InvoiceViewModel invoice, string email)
        {
            List<MailProduct> mailProducts = new List<MailProduct>();

            foreach (var product in invoice.Products)
            {
                mailProducts.Add(new MailProduct
                {
                    Name = product.Name,
                    BrandName = product.BrandName,
                    UnitPrice = product.UnitPrice,
                    Quantity = product.Quantity,
                    ImgUrl = product.ImgUrl
                });
            }

            PendingMail pendingMail = new PendingMail
            {
                Recipient = email,
                MailProducts = mailProducts
            };

            return pendingMail;
        }

        public InvoiceViewModel MapPendingMailToInvoice(PendingMail pendingMail)
        {
            var invoice = new InvoiceViewModel();


            invoice.Recipient = pendingMail.Recipient;
            invoice.Id = pendingMail.Id;

            if (pendingMail.MailProducts is not null)
            {
                foreach (var product in pendingMail.MailProducts)
                    invoice.Products.Add(new ProductModel
                    {
                        BrandName = product.BrandName,
                        Name = product.Name,
                        Quantity = product.Quantity,
                        ImgUrl = product.ImgUrl,
                        UnitPrice = product.UnitPrice
                    });
            }

            return invoice;
        }
    }
}
