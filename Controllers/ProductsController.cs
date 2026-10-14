
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ProductService _productService;

    public ProductsController(AppDbContext context, ProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        
        return Ok(new { data = products, message = "Products retrieved successfully." , status = "success" });
    }

    // GET: api/products/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product is null)
            return NotFound(new { message = "Product not found.", status = "error" });

        return Ok(new { data = product, message = "Product retrieved successfully." , status = "success" });
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        var createdProduct = await _productService.CreateProductAsync(product);
        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProduct.Id },
            new { data = createdProduct, message = "Product created successfully." , status = "success" }
        );
    }

    // PUT: api/products/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        if (id != product.Id)
            return BadRequest(new { message = "ID mismatch.", status = "error" });

        var result = await _productService.UpdateProductAsync(id, product);

        if (!result)
            return NotFound(new { message = "Product not found.", status = "error" });

        return Ok(new { data = product, message = "Product updated successfully." , status = "success" });
    }

    // DELETE: api/products/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteProductAsync(id);

        if (!result)
            return NotFound(new { message = "Product not found.", status = "error" });

        return Ok(new { data = result, message = "Product deleted successfully." , status = "success" });
    }
}
