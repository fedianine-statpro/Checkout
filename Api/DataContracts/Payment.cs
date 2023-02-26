using System.ComponentModel.DataAnnotations;
using Api.DataContracts.Dto;

namespace Api.DataContracts
{
    public record Payment : PaymentBase
    {
        public Payment()
        {

        }

        public Payment(CardDetails cardDetails, decimal amount, string currency, PaymentStatus status)
        {
            CardNumber = cardDetails.CardNumber;
            ExpiryYear = cardDetails.ExpiryYear;
            ExpiryMonth = cardDetails.ExpiryMonth;
            Status = status.ToString();
            Amount = amount;
            Currency = currency;
            Id = Guid.NewGuid();
        }

        private string? _cardNumber;

        public string? CardNumber
        {
            get => _cardNumber;
            set => _cardNumber = MaskCardNumber(value);
        }

        public string CVV => "***";
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }

        [Key]
        public Guid Id { get; init; }

        public string Status { get; init; }


        // Helper method to mask card number
        private string? MaskCardNumber(string? cardNumber)
        {
            if (cardNumber == null)
                return null;
            return "************" + cardNumber.Substring(12);
        }
    }
}
