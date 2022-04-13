using AutoMapper;
using DnsClient;
using DotNetHomeWork.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotNetHomeWork.Extensions;
using DotNetHomeWork.Infrastructure;
using DotNetHomeWork.Infrastructure.Models;
using DotNetHomeWork.Infrastructure.Repositories;
using DotNetHomeWork.Models;
using MongoDB.Driver;

namespace DotNetHomeWork
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.RegisterDependencies();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Будет падать пока БД не захостирована
            // EnsureBankIntegrationsCollectionExists();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void EnsureBankIntegrationsCollectionExists()
        {
            var mongoClient = new MongoClient(DBSettings.ConnectionString);
            var repository = new ProductRepository(mongoClient, Core.Logging.Logging.GetLog());
            repository.EnsureCollectionExists();
        }
    }
}
