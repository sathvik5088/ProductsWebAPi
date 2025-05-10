using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductswebAPI.Data;
using ProductswebAPI.Models;

namespace ProductswebAPI.Controllers{

  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase{

    private readonly ApplicationDbContext context;
    public ProductsController(ApplicationDbContext context)    {
      this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts(){
      return await context.Products.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id){
      var product = await context.Products.FindAsync(id); 

      if(product == null){
        return NotFound();
      }
      return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> InsertProduct(Product prd){
      await context.Products.AddAsync(prd);
      await context.SaveChangesAsync();

      return Ok(prd);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product prd){
      if(id !=prd.Id){
        return BadRequest();
      }

      context.Entry(prd).State = EntityState.Modified;
      await context.SaveChangesAsync();

      return Ok(prd);
    }

    [HttpDelete("{id}")]

    public async Task<ActionResult<Product>> DeleteProduct(int id){
      var prd = await context.Products.FindAsync(id);

      if(prd == null){
        return NotFound();
      }

      context.Products.Remove(prd);
      await context.SaveChangesAsync();

      return Ok();
    }

  }
}