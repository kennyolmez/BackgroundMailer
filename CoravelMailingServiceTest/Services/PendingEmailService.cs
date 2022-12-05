using Coravel.Mailer.Mail.Interfaces;
using Coravel.Queuing.Interfaces;
using CoravelMailingServiceTest.Data;
using CoravelMailingServiceTest.Data.Entities;
using CoravelMailingServiceTest.Mailables;
using CoravelMailingServiceTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace CoravelMailingServiceTest.Services
{
    public class PendingEmailService : IHostedService, IDisposable
    {
        private readonly IMailer _mailer;
        private readonly IQueue _queue;
        public readonly CancellationTokenSource _cts = new();
        private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(30000));
        private Task? RequeueMailTask;
        private readonly IServiceScopeFactory _scopeFactory;
        public PendingEmailService(IServiceScopeFactory scopeFactory, IMailer mailer, IQueue queue)
        {
            _scopeFactory = scopeFactory;
            _mailer = mailer;
            _queue = queue;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            RequeueMailTask = ResendPendingMails();
        }

        private async Task ResendPendingMails()
        {
            while (await _timer.WaitForNextTickAsync(_cts.Token))
            {
                Console.WriteLine("Trying to send pending mail...");


                // Need to create a new scope because AddDbContext registers ApplicationDbContext as scoped by defualt
                // Which cannot be resolved in a IHostedService which is a singleton
                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    var mailSvc = scope.ServiceProvider.GetService<MailServices>();

                    // Should deal with this potentailly being null
                    var pendingMail = await db.PendingMails.Include(x => x.MailProducts).Select(x => x).FirstOrDefaultAsync();

                    if(pendingMail != null)
                    {
                        var invoice = mailSvc!.MapPendingMailToInvoice(pendingMail);

                        try
                        {
                            await _mailer.SendAsync(new NewUserViewMailable(invoice));

                            db.Remove(pendingMail);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            } 
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            if(RequeueMailTask is null)
            {
                return;
            }

            _cts.Cancel();
            _cts.Dispose();

            Console.WriteLine("Task was terminated.");
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
