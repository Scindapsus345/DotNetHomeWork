using DotNetHomeWork.Core.Interfaces;
using DotNetHomeWork.Core.Models;
using DotNetHomeWork.Infrastructure.Models;
using System.Threading.Tasks;
using DotNetHomeWork.Infrastructure.Repositories;

namespace DotNetHomeWork.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public async Task<Product> AddProductAsync(string name, int price)
        {
            var product = await _productRepository.AddOrUpdateProductAsync(new ProductAddModel { Name = name, Price = price });
            return new Product { Name = product.Name, Price = product.Price };
        }

        public async Task DeleteProductAsync(string name)
        {
            await _productRepository.DeleteProductAsync(new ProductDeleteModel { Name = name });
        }

        public async Task<Product> GetProductAsync(string name)
        {
            var product = await _productRepository.GetProductAsync(name).ConfigureAwait(false);
            return new Product {Name = product.Name, Price = product.Price};
        }

        public async Task<bool> TryBuyProductAsync(string name, int moneyAmount)
        {
            var product = await _productRepository.GetProductAsync(name).ConfigureAwait(false);
            if (product == null || product.Price > moneyAmount)
                return false;
            return true;
        }

        public async Task<Product> UpdateProductPriceAsync(string name, int newPrice)
        {
            throw new System.NotImplementedException();
        }
    }
}
