using HPlusSport.API.Data;
using HPlusSport.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPlusSport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _context;
        public ProductsController(ShopContext context)
        {
            _context= context;
            //if seed() is not run yet, run seed().
            _context.Database.EnsureCreated();
        }
        [HttpGet]
        //following two method of GetALLProducts are same faction
        //method 1 getAll
        //public IEnumerable<Product> GetAllProducts()
        //{
        //    return _context.Products.ToList();
        //}

        //method 2 getAll
        public async Task <ActionResult> GetAllProducts() {
            var products = _context.Products;
            return Ok(await products.ToListAsync());
        }


        //following two httpGet is same
        //[HttpGet][Route("/api/products/{id}")]
        [HttpGet("{id}")]
        public async Task <ActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct",
                                    new { id = product.Id },
                                    product);
        }

        [HttpPut("{id}")]
        public async Task <ActionResult> PutProduct(int id, Product product)
        {
            if(id !=product.Id)
            {
                return BadRequest();    
            }
            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)//maybe product is modified by other API call sametime
            {
                //try to see if the product is exist or already deleted
                if (!_context.Products.Any(p => p.Id == id))
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

        [HttpDelete("{id}")]
        public async Task <ActionResult<Product>> DelelteProduct(int id)
        {
            var product=await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        [HttpPost]
        [Route("Delete")]
        public async Task <ActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var needDeleted = new List<Product>();
            foreach (int id in ids)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null) return NotFound();
                needDeleted.Add(product);
            }
            _context.Products.RemoveRange(needDeleted);
            await _context.SaveChangesAsync();

            return Ok(needDeleted);
        }
    }
}
