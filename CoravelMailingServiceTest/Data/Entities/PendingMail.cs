using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoravelMailingServiceTest.Data.Entities
{
    public class PendingMail
    {
        public int Id { get; set; }
        public string Recipient { get; set; }
        public List<MailProduct> MailProducts { get; set; }
    }

    public class PendingMailConfiguration : IEntityTypeConfiguration<PendingMail>
    {
        public void Configure(EntityTypeBuilder<PendingMail> builder)
        {
            builder.HasMany(b => b.MailProducts).WithOne(x => x.Mail);
        }
    }
}
