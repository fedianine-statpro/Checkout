using Api.DataContracts;
using Api.DataContracts.Dto;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly BankSimulator.IPayment _bankSimulator;
        private readonly IPaymentReadRepository _paymentReadRepository;
        private readonly IPaymentWriteRepository _paymentWriteRepository;

        public PaymentController(ILogger<PaymentController> logger, BankSimulator.IPayment bankSimulator,
            IPaymentReadRepository paymentReadRepository, IPaymentWriteRepository paymentWriteRepository)
        {
            _logger = logger;
            _bankSimulator = bankSimulator;
            _paymentReadRepository = paymentReadRepository;
            _paymentWriteRepository = paymentWriteRepository;
        }

        // Payment processing endpoint
        [HttpPost]
        [ProducesResponseType(typeof(PaymentResponse), Status200OK)]
        [ProducesResponseType(typeof(ObjectResult), Status400BadRequest)]
        [ProducesResponseType(typeof(ObjectResult), Status500InternalServerError)]
        public async Task<ActionResult<PaymentResponse>> ProcessPayment(PaymentRequest paymentRequest)
        {
            try
            {
                // Validate payment data
                // TODO: An improvement can be to return what specifically was invalid about the data
                if (!paymentRequest.IsValid())
                    return BadRequest("Invalid payment details.");

                // Simulate payment processing with CKO bank simulator
                var bankResponse = await _bankSimulator.ProcessPayment(paymentRequest.Card.CardNumber, paymentRequest.Card.ExpiryMonth, paymentRequest.Card.ExpiryYear, paymentRequest.Amount, paymentRequest.Currency, paymentRequest.Card.CVV);

                // Map bank response to payment status
                var paymentStatus = bankResponse.Response == BankSimulator.PaymentStatus.Approved ? PaymentStatus.Successful : PaymentStatus.Unsuccessful;
                Payment payment = new Payment(paymentRequest.Card, paymentRequest.Amount, paymentRequest.Currency, paymentStatus);

                // Save payment to database
                await _paymentWriteRepository.AddAsync(payment);

                // Return payment status to merchant
                return new PaymentResponse(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment could not processed!");
                return Problem("Internal error. Please check the logs.");
            }
        }

        // Payment retrieval endpoint
        [HttpGet]
        [ProducesResponseType(typeof(PaymentResponse), Status200OK)]
        [ProducesResponseType(typeof(ObjectResult), Status404NotFound)]
        [ProducesResponseType(typeof(ObjectResult), Status500InternalServerError)]
        public async Task<ActionResult<PaymentResponse>> GetPayment(Guid id)
        {
            try
            {
                // Retrieve payment from database
                var payment = await _paymentReadRepository.GetAsync(id);

                if (payment == null)
                    return NotFound();

                // Return payment details to merchant
                return new PaymentResponse(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment could not retrieved!", id);
                return Problem("Internal error. Please check the logs.");
            }
        }
    }
}
