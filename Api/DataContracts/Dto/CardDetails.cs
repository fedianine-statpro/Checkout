namespace Api.DataContracts.Dto
{
    public record CardDetails
    {
        public string? CardNumber { get; set; }
        public string? CVV { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
    }
}
