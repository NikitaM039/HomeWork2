
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HomeWork2.Abstraction;
using HomeWork2.DB;
using HomeWork2.Repo;
using Microsoft.Extensions.FileProviders;

namespace HomeWork2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(MappingProFile));
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            var config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            var cfg = config.Build();

            builder.Host.ConfigureContainer<ContainerBuilder>(cbb =>
            {
                cbb.Register(cb => new ProductContext(cfg.GetConnectionString("db"))).InstancePerDependency();
            });

            builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
            {
                cb.RegisterType<ProductRepository>().As<IProductRepository>();
            });
            builder.Services.AddMemoryCache(o => o.TrackStatistics = true);

            //builder.Services.AddSingleton<IProductRepository, ProductRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
            Directory.CreateDirectory(staticFilesPath);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilesPath),
                RequestPath = "/static"
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}