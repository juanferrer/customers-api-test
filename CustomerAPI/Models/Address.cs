using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace CustomerAPI.Models
{
    public class Address
    {
        public required Guid Id { get; set; }
        public required string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public required string Town { get; set; }
        public string? County { get; set; }
        public required string Postcode { get; set; }
        public required string Country { get; set; }

        public Address() { }

        public static implicit operator Address(DbAddress dbAddress)
        {
            return new Address
            {
                Id = dbAddress.Id,
                AddressLine1 = dbAddress.AddressLine1,
                AddressLine2 = dbAddress.AddressLine2,
                Town = dbAddress.Town,
                County = dbAddress.County,
                Postcode = dbAddress.Postcode,
                Country = dbAddress.Country
            };
        }

        public static explicit operator DbAddress(Address address)
        {
            return new DbAddress
            {
                Id = address.Id,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                Town = address.Town,
                County = address.County,
                Postcode = address.Postcode,
                Country = address.Country,
            };
        }
    }
}
