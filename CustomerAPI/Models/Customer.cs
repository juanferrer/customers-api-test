namespace CustomerAPI.Models
{
    public class Customer
    {
        public required Guid Id { get; set; }

        /// <summary>
        /// Up to 20 characters
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Up to 50 characters
        /// </summary>
        public required string Forename { get; set; }

        /// <summary>
        /// Up to 50 characters
        /// </summary>
        public required string Surname { get; set; }

        /// <summary>
        /// Up to 75 characters
        /// </summary>
        public required string EmailAddress { get; set; }

        /// <summary>
        /// Up to 15 characters
        /// </summary>
        public required string MobileNumber { get; set; }

        /// <summary>
        /// First in the list is the main address
        /// </summary>
        public required List<Address> Addresses { get; set; }

        public bool Active { get; set; }
    }
}
