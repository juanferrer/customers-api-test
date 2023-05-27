namespace CustomerAPI.Models
{
    public class Address
    {
        public required Guid Id { get; set; }

        /// <summary>
        /// Up to 80 characters
        /// </summary>
        public required string AddressLine1 { get; set; }

        /// <summary>
        /// Up to 80 characters
        /// </summary>
        public string? AddressLine2 { get; set; }

        /// <summary>
        /// Up to 50 characters
        /// </summary>
        public required string Town { get; set; }

        /// <summary>
        /// Up to 50 characters
        /// </summary>
        public string? County { get; set; }

        /// <summary>
        /// Up to 10 characters
        /// </summary>
        public required string Postcode { get; set; }

        /// <summary>
        /// Defaults to UK
        /// </summary>
        public required string Country { get; set; }
    }
}
