using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DotNetHomeWork.Core.Interfaces;
using DotNetHomeWork.Models;

namespace DotNetHomeWork.Controllers
{
    [Route("api/products")]
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("{name:string}")]
        public async Task<IHttpActionResult> GetProduct(string name)
        {
            var product = await _productService.GetProductAsync(name).ConfigureAwait(false);
            return Ok(new ProductModel { Name = product.Name, Price = product.Price });
        }

        [HttpPost]
        [Route("{name:string}")]
        public async Task<IHttpActionResult> CreateProduct(string name, int price)
        {
            var product = await _productService.AddProductAsync(name, price).ConfigureAwait(false);
            var location = $"{Request.RequestUri.GetLeftPart(UriPartial.Path)}";

            return Created(location, product);
        }

        [HttpPut]
        [Route("{name:string}")]
        public async Task<IHttpActionResult> UpdatePrice(string name, int newPrice)
        {
            var product = await _productService.UpdateProductPriceAsync(name, newPrice).ConfigureAwait(false);
            return Ok(new ProductModel { Name = product.Name, Price = product.Price });
        }

        [HttpDelete]
        [Route("{name:string}")]
        public async Task<IHttpActionResult> DeleteProduct(string name)
        {
            await _productService.DeleteProductAsync(name);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("{name:string}/trybuy")]
        public async Task<IHttpActionResult> TryBuyProduct(string name, int moneyAmount)
        {
            var productIsBought = await _productService.TryBuyProductAsync(name, moneyAmount).ConfigureAwait(false);

            return Ok(productIsBought);
        }
    }
}
