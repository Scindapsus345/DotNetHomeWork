using System;
using System.Linq;
using AutoMapper;
using DnsClient;
using DotNetHomeWork.Core.Interfaces;
using DotNetHomeWork.Core.Models;
using DotNetHomeWork.Core.Services;
using DotNetHomeWork.Dto;
using DotNetHomeWork.Infrastructure;
using DotNetHomeWork.Infrastructure.Models;
using DotNetHomeWork.Infrastructure.Repositories;
using DotNetHomeWork.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DotNetHomeWork.Extensions
{
	public static class ServiceCollectionExtensions
    {
        public static IServiceProvider RegisterDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterControllers();

            return serviceCollection
                .AddSingleton(sp => Core.Logging.Logging.GetLog())
                .AddSingleton(sp =>
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Product, ProductModel>();
                        cfg.CreateMap<ProductDto, Product>();
                        cfg.CreateMap<ProductStored, Product>();
                    });
                    return config.CreateMapper();
                })
                .AddScoped<IMongoClient, MongoClient>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped(sp => new MongoClient(DBSettings.ConnectionString))
                .BuildServiceProvider();
        }

        private static void RegisterControllers(this IServiceCollection serviceCollection)
        {
            var apiApplicationTypes = typeof(Startup).Assembly.DefinedTypes;
            var controllerTypes = apiApplicationTypes
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract)
                .Select(type => type.AsType());

            foreach (var controllerType in controllerTypes)
                serviceCollection.AddScoped(controllerType);
        }
	}
}
