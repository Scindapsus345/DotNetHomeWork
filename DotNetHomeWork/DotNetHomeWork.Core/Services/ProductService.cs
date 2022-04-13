using DotNetHomeWork.Core.Interfaces;
using DotNetHomeWork.Core.Models;
using DotNetHomeWork.Infrastructure.Models;
using System.Threading.Tasks;
using DotNetHomeWork.Infrastructure.Repositories;
using Vostok.Logging.Abstractions;

namespace DotNetHomeWork.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILog log;

        public ProductService(IProductRepository productRepository, ILog log)
        {
            _productRepository = productRepository;
            this.log = log;
        }

        public async Task<Product> AddProductAsync(string name, int price)
        {
            var product = await _productRepository.AddOrUpdateProductAsync(new ProductAddModel { Name = name, Price = price });
            log.Info($"Товар {name} создан");
            return new Product { Name = product.Name, Price = product.Price };
        }

        public async Task DeleteProductAsync(string name)
        {
            await _productRepository.DeleteProductAsync(new ProductDeleteModel { Name = name });
            log.Info($"Товар {name} удален");
        }

        public async Task<Product> GetProductAsync(string name)
        {
            var product = await _productRepository.GetProductAsync(name).ConfigureAwait(false);
            if (product == null) return null;
            return new Product {Name = product.Name, Price = product.Price};
        }

        public async Task<bool> TryBuyProductAsync(string name, int moneyAmount)
        {
            var product = await _productRepository.GetProductAsync(name).ConfigureAwait(false);
            if (product == null)
            {
                log.Info($"Товара {name} не существует");
                return false;
            }

            if (product.Price > moneyAmount)
            {
                log.Info($"недостаточно денег для товара {name}");
                return false;
            }
            log.Info($"Товар {name} куплен");
            await _productRepository.DeleteProductAsync(new ProductDeleteModel { Name = name }).ConfigureAwait(false);
            return true;
        }

        public async Task<Product> UpdateProductPriceAsync(string name, int newPrice)
        {
            var product = await _productRepository.AddOrUpdateProductAsync(new ProductAddModel { Name = name, Price = newPrice });
            log.Info($"Цена на {name} обновлена ({newPrice})");
            return new Product { Name = product.Name, Price = product.Price };
        }
    }
}
