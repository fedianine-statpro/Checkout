namespace Api.DataContracts.Dto
{
    public sealed record PaymentRequest : PaymentBase
    {
        public CardDetails Card { get; set; }

        public static bool IsValidCardNumber(string cardNumber)
        {
            // Card number validation logic, e.g. check length, Luhn algorithm, etc.
            if (cardNumber.Length != 16)
                return false;
            return true; // Return true if the card number is valid, otherwise false
        }

        public static bool IsValidExpiryMonth(int expiryMonth)
        {
            return expiryMonth >= 1 && expiryMonth <= 12;
        }

        public static bool IsValidExpiryYear(int expiryYear)
        {
            return expiryYear >= DateTime.UtcNow.Year;
        }

        public static bool IsValidExpiryYearAndMonth(int expiryYear, int expiryMonth)
        {
            return expiryYear >= DateTime.UtcNow.Year &&
                   expiryMonth >= DateTime.UtcNow.Month;
        }

        public static bool IsValidAmount(decimal amount)
        {
            return amount > 0;
        }

        public static bool IsValidCurrency(string currency)
        {
            // Currency validation logic, e.g. check if it's a valid ISO 4217 code, etc.
            return new []
            {
                "AFN", "DZD", "ARS", "AMD", "AWG", "AUD", "AZN", "BSD", "BHD", "THB", "PAB",
                "BBD", "BYN", "BZD", "BMD", "VEF", "BOB", "BRL", "BND", "BGN", "BIF", "CAD",
                "CVE", "KYD", "GHS", "XOF", "XAF", "XPF", "CLP", "COP", "KMF", "CDF", "BAM",
                "NIO", "CRC", "HRK", "CUP", "CZK", "GMD", "DKK", "MKD", "DJF", "STD", "DOP",
                "VND", "XCD", "EGP", "SVC", "ETB", "EUR", "FKP", "FJD", "HUF", "GIP", "HTG",
                "PYG", "GNF", "GYD", "HKD", "UAH", "ISK", "INR", "IRR", "IQD", "JMD", "JOD",
                "KES", "PGK", "LAK", "EEK", "KWD", "MWK", "AOA", "MMK", "GEL", "LVL", "LBP",
                "ALL", "HNL", "SLL", "LRD", "LYD", "SZL", "LTL", "LSL", "MGA", "MYR", "TMT",
                "MUR", "MZN", "MXN", "MDL", "MAD", "NGN", "ERN", "NAD", "NPR", "ANG", "ILS",
                "RON", "TWD", "NZD", "BTN", "KPW", "NOK", "PEN", "MRO", "TOP", "PKR", "MOP",
                "UYU", "PHP", "GBP", "BWP", "QAR", "GTQ", "ZAR", "OMR", "KHR", "MVR", "IDR",
                "RUB", "RWF", "SHP", "SAR", "RSD", "SCR", "SGD", "SBD", "KGS", "SOS", "TJS",
                "LKR", "SDG", "SRD", "SEK", "CHF", "SYP", "BDT", "WST", "TZS", "KZT", "TTD",
                "MNT", "TND", "TRY", "AED", "UGX", "CLF", "USD", "UZS", "VUV", "KRW", "YER",
                "JPY", "CNY", "ZMW", "ZWL", "PLN"
            }.Contains(currency, StringComparer.InvariantCultureIgnoreCase);
        }

        public static bool IsValidCVV(string cvv)
        {
            // CVV validation logic, e.g. check length, etc.
            if (cvv.Length != 3)
                return false;
            return true; // Return true if the CVV is valid, otherwise false
        }

        public bool IsValid()
        {
            return IsValidCardNumber(Card.CardNumber) &&
                   IsValidExpiryMonth(Card.ExpiryMonth) &&
                   IsValidExpiryYear(Card.ExpiryYear) &&
                   IsValidExpiryYearAndMonth(Card.ExpiryYear, Card.ExpiryMonth) &&
                   IsValidAmount(Amount) &&
                   IsValidCurrency(Currency) &&
                   IsValidCVV(Card.CVV);
        }
    }
}
