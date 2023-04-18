using BLL.Interfaces;
using Braintree;
using BraintreeApp.MVC.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BraintreeApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBraintreeService _braintreeService;

        public HomeController(ILogger<HomeController> logger, IBraintreeService braintreeService)
        {
            _logger = logger;
            _braintreeService = braintreeService;
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

        public IActionResult Index()
        {
            try
            {
                var gateway = _braintreeService.GetGateway();
                var clientToken = gateway.ClientToken.Generate();
                ViewBag.ClientToken = clientToken;
            }
            catch (Exception ex)
            {
                _logger.LogError("Invalid credentials.", ex);
            }

            var model = new BookPurchaseVM() { Thumbnail = "/Images/credit-card.jpg" };

            return View(model);
        }

        public IActionResult Create(BookPurchaseVM model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var gateway = _braintreeService.GetGateway();
            try
            {
                var price = Convert.ToDecimal(model.Price);
            }
            catch
            {
                _logger.LogError("Invalid price format.");
                ViewBag.Result = false;
                return View("Confirmation");
            }

            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(model.Price),
                PaymentMethodNonce = model.Nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            ViewBag.Result = result.IsSuccess();

            return View("Confirmation");
        }
    }
}