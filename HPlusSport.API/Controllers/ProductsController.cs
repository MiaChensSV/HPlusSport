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
    }
}
