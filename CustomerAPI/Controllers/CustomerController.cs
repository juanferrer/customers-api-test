using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerAPI.Models;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CustomerAPI.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerContext _context;

        public CustomerController(CustomerContext context)
        {
            _context = context;
        }

        // GET: api/customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            if (_context.Customers == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var dbCustomers = await _context.Customers.ToListAsync();
            var customers = new List<Customer>();

            foreach (var dbCustomer in dbCustomers)
            {
                var addresses = await GetAddressesForCustomer(dbCustomer);
                var newCustomer = (Customer)dbCustomer;
                newCustomer.Addresses = addresses;
                customers.Add(newCustomer);
            }

            return customers;
        }

        // GET: api/customer/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetActiveCustomers()
        {
            if (_context.Customers == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var dbCustomers = await _context.Customers.Where(c => c.Active).ToListAsync();
            var customers = new List<Customer>();

            foreach (var dbCustomer in dbCustomers)
            {
                var addresses = await GetAddressesForCustomer(dbCustomer);
                var newCustomer = (Customer)dbCustomer;
                newCustomer.Addresses = addresses;
                customers.Add(newCustomer);
            }

            return customers;
        }

        // GET: api/customer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(Guid id)
        {
            if (_context.Customers == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var dbCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);


            if (dbCustomer == null)
            {
                return NotFound();
            }

            var addresses = await GetAddressesForCustomer(dbCustomer);
            var newCustomer = (Customer)dbCustomer;
            newCustomer.Addresses = addresses;

            return newCustomer;
        }

        // PUT: api/customer/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(Guid id, Customer customer)
        {
            if (id != customer.Id || customer == null)
            {
                return BadRequest();
            }

            if (_context.Customers == null)
            {
                return Problem("Entity set 'CustomerContext.Customers' is null.");
            }

            if (_context.Addresses == null)
            {
                return Problem("Entity set 'CustomerContext.Addresses' is null.");
            }

            if (customer.Addresses == null || customer.Addresses.Count == 0)
            {
                return Problem(
                    title: "Forbidden",
                    statusCode: StatusCodes.Status403Forbidden,
                    detail: "Customer must have at least one address");
            }

            // Need to put addresses in a different table
            var addresses = customer.Addresses;
            var dbCustomer = (DbCustomer)customer;
            dbCustomer.MainAddress = addresses[0].Id;
            _context.Entry(dbCustomer).State = EntityState.Modified;

            _context.Customers.Update(dbCustomer);

            var dbAddresses = new List<DbAddress>();
            foreach (var address in addresses)
            {
                var dbAddress = (DbAddress)address;
                dbAddress.Customer = customer.Id;
                dbAddresses.Add(dbAddress);
            }
            _context.Addresses.UpdateRange(dbAddresses);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/customer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            if (_context.Customers == null)
            {
                return Problem("Entity set 'CustomerContext.Customers' is null.");
            }

            if (_context.Addresses == null)
            {
                return Problem("Entity set 'CustomerContext.Addresses' is null.");
            }

            if (customer.Addresses == null || customer.Addresses.Count == 0)
            {
                return Problem(
                    title: "Forbidden",
                    statusCode: StatusCodes.Status403Forbidden,
                    detail: "Customer must have at least one address");
            }


            // Need to put addresses in a different table
            var addresses = customer.Addresses;
            var dbCustomer = (DbCustomer)customer;

            if (await _context.Customers.FindAsync(customer.Id) != null)
            {
                return Problem(
                    title: "Forbidden",
                    statusCode: StatusCodes.Status403Forbidden,
                    detail: "Customer already exists");
            }

            await _context.Customers.AddAsync(dbCustomer);

            var dbAddresses = new List<DbAddress>();
            foreach (var address in addresses)
            {
                var dbAddress = (DbAddress)address;
                dbAddress.Customer = customer.Id;
                dbAddresses.Add(dbAddress);
            }
            await _context.Addresses.AddRangeAsync(dbAddresses);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/customer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            if (_context.Customers == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var dbAddresses = await _context.Addresses.Where(a => a.Customer == id).ToListAsync();
            _context.Addresses.RemoveRange(dbAddresses);

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(Guid id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<List<Address>> GetAddressesForCustomer(DbCustomer dbCustomer)
        {
            var dbAddresses = await _context.Addresses.Where(a => a.Customer == dbCustomer.Id).ToListAsync();
            var addresses = new List<Address>();
            addresses.Add(dbAddresses.Where(a => a.Id == dbCustomer.MainAddress).First());
            foreach (var dbAddress in dbAddresses)
            {
                if (dbAddress.Id != dbCustomer.MainAddress)
                {
                    addresses.Add(dbAddress);
                }
            }
            return addresses;
        }
    }
}
