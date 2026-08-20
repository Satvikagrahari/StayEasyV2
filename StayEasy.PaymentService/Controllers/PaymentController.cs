using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayEasy.PaymentService.Data;
using StayEasy.PaymentService.Entities;
using StayEasy.PaymentService.Models;
namespace StayEasy.PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly PaymentDbContext _dbContext;
        public PaymentController(ILogger<PaymentController> logger, PaymentDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost("Process")]
        public async Task<IActionResult> ProcessPayment(PaymentRequest request)
        {
            _logger.LogInformation($"Processing payment of {request.Amount} for Booking {request.BookingId}");

            // Mask card number for security (only show last 4 digits, e.g. ************1234)
            string maskedCard = request.CardNumber.Length >= 4
                ? new string('*', request.CardNumber.Length - 4) + request.CardNumber.Substring(request.CardNumber.Length - 4) : "****";


            // Basic validation
            if (string.IsNullOrWhiteSpace(request.CardNumber) || request.CardNumber.Length < 15)
            {
                return await SaveLogAndReturn(new PaymentLog
                {
                    BookingId = request.BookingId,
                    Amount = request.Amount,
                    CardHolderName = request.CardHolderName,
                    MaskedCardNumber = maskedCard,
                    IsSuccess = false,
                    ErrorMessage = "Invalid card number length."
                }, BadRequest);
            }
            // Simulated business logic
            if (request.CardNumber.StartsWith("4"))
            {
                string transactionId = $"TXN_{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
                _logger.LogInformation($"Payment SUCCESS. Transaction ID: {transactionId}");
                return await SaveLogAndReturn(new PaymentLog
                {
                    BookingId = request.BookingId,
                    Amount = request.Amount,
                    CardHolderName = request.CardHolderName,
                    MaskedCardNumber = maskedCard,
                    IsSuccess = true,
                    TransactionId = transactionId
                }, Ok);
            }
            _logger.LogWarning($"Payment DECLINED for Booking {request.BookingId}.");
            return await SaveLogAndReturn(new PaymentLog
            {
                BookingId = request.BookingId,
                Amount = request.Amount,
                CardHolderName = request.CardHolderName,
                MaskedCardNumber = maskedCard,
                IsSuccess = false,
                ErrorMessage = "Card declined. Only Visa (starts with 4) is accepted in this simulation."
            }, BadRequest);
        }
        // Helper method to save to DB and format the response automatically
        private async Task<IActionResult> SaveLogAndReturn(PaymentLog log, Func<object, IActionResult> actionResultFunc)
        {
            // 1. Save to database
            _dbContext.PaymentLogs.Add(log);
            await _dbContext.SaveChangesAsync();
            // 2. Return HTTP response back to the Monolith
            var response = new PaymentResponse(log.IsSuccess, log.TransactionId, log.ErrorMessage);
            return actionResultFunc(response);
        }
    }
}
