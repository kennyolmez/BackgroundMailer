using Coravel.Mailer.Mail.Interfaces;
using Coravel.Queuing.Interfaces;
using CoravelMailingServiceTest.Data;
using CoravelMailingServiceTest.Data.Entities;
using CoravelMailingServiceTest.Mailables;
using CoravelMailingServiceTest.Migrations;
using CoravelMailingServiceTest.Models;
using CoravelMailingServiceTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace CoravelMailingServiceTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailer _mailer;
        private readonly MockDataServices _mockDatasvc;
        private readonly IQueue _queue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly MailServices _mailSvc;
        public HomeController(ILogger<HomeController> logger, MailServices mailSvc, IMailer mailer, MockDataServices mockDatasvc, IQueue queue, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _mailer = mailer;
            _mockDatasvc = mockDatasvc;
            _mailSvc = mailSvc;
            _queue = queue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetInvoiceMail(string email)
        {

            if (email is not null)
            {
                InvoiceViewModel model = new InvoiceViewModel
                {
                    Recipient = email,
                    Products = _mockDatasvc.GenerateMockProducts()
                };

                // Queues in memory
                _queue.QueueAsyncTask(async () =>
                {
                    try
                    {
                        await _mailer.SendAsync(new UserViewMailable(model));
                    }
                    catch
                    {

                        // DbContext is a scoped service and is disposed/garbage collected because QueueAsyncTask is executed elsewhere
                        // To resolve this we inject a new IServiceScopeFactory to create a new scope to resolve dependencies 
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

                            var pendingMail = _mailSvc.MapInvoiceToPendingMail(model, email);

                            db!.PendingMails.Add(pendingMail);

                            db.SaveChanges();
                        }
                        Console.WriteLine("Message failed. Mail is added to database to pending status.");
                    }

                });

                var metrics = _queue.GetMetrics();
                Console.WriteLine($"{metrics.RunningCount()} - {metrics.WaitingCount()}");

                return View("Index");
            }

            return View("Index");

        }

        [HttpPost]
        public async Task<IActionResult> SubscribeToNewsletter(string email)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Example()
        {
            return View("~Views/Mail/Invoice.cshtml");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}