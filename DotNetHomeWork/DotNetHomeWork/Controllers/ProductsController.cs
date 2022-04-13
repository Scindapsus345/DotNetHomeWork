using System.Threading.Tasks;
using DotNetHomeWork.Core.Interfaces;
using DotNetHomeWork.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetHomeWork.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<OkObjectResult>> GetProduct(string name)
        {
            var product = await _productService.GetProductAsync(name).ConfigureAwait(false);
            return Ok(new ProductModel { Name = product.Name, Price = product.Price });
        }

        [HttpPost]
        [Route("{name}")]
        public async Task<ActionResult<CreatedResult>> CreateProduct(string name, int price)
        {
            var product = await _productService.AddProductAsync(name, price).ConfigureAwait(false);
            var location = $"{Request.Path.Value}";

            return Created(location, product);
        }

        [HttpPut("{name}")]
        public async Task<ActionResult<OkObjectResult>> UpdatePrice(string name, int newPrice)
        {
            var product = await _productService.UpdateProductPriceAsync(name, newPrice).ConfigureAwait(false);
            return Ok(new ProductModel { Name = product.Name, Price = product.Price });
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult<NoContentResult>> DeleteProduct(string name)
        {
            await _productService.DeleteProductAsync(name);
            return NoContent();
        }

        [HttpPost("{name}/trybuy")]
        public async Task<ActionResult<OkObjectResult>> TryBuyProduct(string name, int moneyAmount)
        {
            var productIsBought = await _productService.TryBuyProductAsync(name, moneyAmount).ConfigureAwait(false);

            return Ok(productIsBought);
        }
    }
}
