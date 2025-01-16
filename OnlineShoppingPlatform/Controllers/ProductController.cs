using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingPlatform.API.Filters;
using OnlineShoppingPlatform.DAL.Entities;
using OnlineShoppingPlatform.DAL.UnitOfWork;

namespace OnlineShoppingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [LoggingFilter]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Get All Products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return Ok(products);
        }

        // Get Product By ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return NotFound("Ürün bulunamadı.");

            return Ok(product);
        }

        // Create Product
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Validasyon hatalarını döndür

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // Update Product
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct == null)
                return NotFound("Ürün bulunamadı.");

            existingProduct.ProductName = product.ProductName;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;

            _unitOfWork.Products.Update(existingProduct);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        // Delete Product
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return NotFound("Ürün bulunamadı.");// Ürün yoksa 404 döner

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.CompleteAsync();// Değişiklikleri kaydet
            return NoContent();// Başarıyla silindiğinde 204 No Content döner
        }

    }
}

