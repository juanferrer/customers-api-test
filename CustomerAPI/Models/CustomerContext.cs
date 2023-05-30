using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerAPI.Models
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options) { }

        public DbSet<DbCustomer> Customers { get; set; }
        public DbSet<DbAddress> Addresses { get; set; }
    }

    public class DbCustomer
    {
        public required Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public required string Title { get; set; }

        [Required]
        [StringLength(50)]
        public required string Forename { get; set; }

        [Required]
        [StringLength(50)]
        public required string Surname { get; set; }

        [Required]
        [StringLength(75)]
        public required string EmailAddress { get; set; }

        [Required]
        [StringLength(15)]
        public required string MobileNumber { get; set; }

        [ForeignKey("MainAddress")]
        public Guid MainAddress { get; set; }

        [Required]
        public bool Active { get; set; }
    }

    public class DbAddress
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(80)]
        public required string AddressLine1 { get; set; }

        [StringLength(20)]
        public string? AddressLine2 { get; set; }

        [Required]
        [StringLength(50)]
        public required string Town { get; set; }

        [StringLength(50)]
        public string? County { get; set; }

        [Required]
        [StringLength(10)]
        public required string Postcode { get; set; }

        [Required]
        [StringLength(2)]
        public required string Country { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public Guid Customer { get; set; }
    }
}
