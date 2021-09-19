using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        private readonly MyStoreContext _context;
        public CustomersController(MyStoreContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var result = await _context.Customers.ToListAsync();
            if (result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer (int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }
            _context.Entry(customer).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!CustomerExists(id))
                {
                    return NotFound();
                }else
                {
                    throw;
                }
            }
            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return customer;
        }




        [HttpPost]
        public async Task<ActionResult<Customer>> InsertCustomer(Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Please insert Customer informations");
            }
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return Ok();
        }



        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }



    }
}
