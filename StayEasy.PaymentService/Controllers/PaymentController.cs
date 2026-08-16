using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayEasy.PaymentService.Models;
namespace StayEasy.PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Process")]
        public IActionResult ProcessPayment(PaymentRequest request)
        {
            _logger.LogInformation($"Processing payment of {request.Amount} for Booking {request.BookingId}");

            //basic validation
            if(string.IsNullOrWhiteSpace(request.CardNumber) || request.CardNumber.Length < 15)
            {
                return BadRequest(new PaymentResponse(false, null, "Invalid card number length."));
            }

            //simulated buisness logic
            // If the card starts with '4' (Visa), approve it!
            if (request.CardNumber.StartsWith("4"))
            {
                // Generate a fake transaction ID
                string transactionId = $"TXN_{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

                _logger.LogInformation($"Payment SUCCESS. Transaction ID: {transactionId}");

                return Ok(new PaymentResponse(true, transactionId, null));
            }
            // Otherwise, decline it.
            _logger.LogWarning($"Payment DECLINED for Booking {request.BookingId}.");

            return BadRequest(new PaymentResponse(false, null, "Card declined. Only Visa (starts with 4) is accepted in this simulation."));
        }
    }
}
