using Bogus.DataSets;
using CoravelMailingServiceTest.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoravelMailingServiceTest.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<PendingMail> PendingMails { get; set; }
        public DbSet<MailProduct> MailProducts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(PendingMail).Assembly);
        }
    }
}
