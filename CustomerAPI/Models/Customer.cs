using System.Diagnostics.CodeAnalysis;

namespace CustomerAPI.Models
{
    public class Customer
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Forename { get; set; }
        public required string Surname { get; set; }
        public required string EmailAddress { get; set; }
        public required string MobileNumber { get; set; }
        public List<Address>? Addresses { get; set; }
        public bool Active { get; set; }

        public Customer() { }

        //[SetsRequiredMembers]
        //public Customer(DbCustomer dbCustomer, List<DbAddress> dbAddresses)
        //{
        //    Id = dbCustomer.Id;
        //    Title = dbCustomer.Title;
        //    Forename = dbCustomer.Forename;
        //    Surname = dbCustomer.Surname;
        //    EmailAddress = dbCustomer.EmailAddress;
        //    MobileNumber = dbCustomer.MobileNumber;
        //    Addresses = dbAddresses.Select(a => new Address(a)).ToList();
        //    Active = dbCustomer.Active;
        //}

        public static implicit operator Customer (DbCustomer dbCustomer)
        {
            return new Customer()
            {
                Id = dbCustomer.Id,
                Title = dbCustomer.Title,
                Forename = dbCustomer.Forename,
                Surname = dbCustomer.Surname,
                EmailAddress = dbCustomer.EmailAddress,
                MobileNumber = dbCustomer.MobileNumber,
                Addresses = new List<Address>(),
                Active = dbCustomer.Active
            };
        }

        public static explicit operator DbCustomer(Customer customer)
        {
            return new DbCustomer()
            {
                Id = customer.Id,
                Title = customer.Title,
                Forename = customer.Forename,
                Surname = customer.Surname,
                EmailAddress = customer.EmailAddress,
                MobileNumber = customer.MobileNumber,
                MainAddress = customer.Addresses?[0].Id ?? Guid.Empty,
                Active = customer.Active
            };
        }
    }
}
