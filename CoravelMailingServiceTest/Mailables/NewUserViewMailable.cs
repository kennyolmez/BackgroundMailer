using Coravel.Mailer.Mail;
using CoravelMailingServiceTest.Models;

namespace CoravelMailingServiceTest.Mailables
{
    public class NewUserViewMailable : Mailable<InvoiceViewModel>
    {
        private InvoiceViewModel model;

        public NewUserViewMailable(InvoiceViewModel _model) => model = _model;

        public override void Build()
        {
                To(model.Recipient)
                .From("gamifywebshop@gmail.com")
                .View("~/Views/Mail/Invoice.cshtml", model);
        }
    }
}
