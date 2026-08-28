
using E_Commerce.API.Extensions;
using E_Commerce.Infrastructure;
using E_Commerce.Application;
using System.Threading.Tasks;
using E_Commerce.Application.Profiles;
using System.Net.NetworkInformation;
using Microsoft.Extensions.FileProviders;
using E_Commerce.Infrastructure.Identity.Entities;
using E_Commerce.Infrastructure.Services;
using E_Commerce.Application.common;

namespace E_Commerce.API
{
    public class Program
    {
       
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        });            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.ApplicationServicesRegistration();
            builder.Services.Configure<UrlSettings>(builder.Configuration.GetSection("UrlSettings"));
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
            builder.Services.Configure<PaymentGatewaySettings>(builder.Configuration.GetSection("Stripe"));
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:3000", // React / Next.js
                            "http://localhost:5173", // Vite (React, Vue, Svelte)
                            "http://localhost:4200"  // Angular
                          )
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // Required if using cookies or auth headers
                });
            });

            var app = builder.Build();

          await  app.SeedAndMigrateDataAsync();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce API v1");
                c.RoutePrefix = "swagger";
            });


            
           
           
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath,"Files")),
                RequestPath = "/Files"
            });

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
    

         


            app.UseAuthentication();
            app.UseAuthorization();

          


            app.MapControllers();               

            app.Run();
        }
    }
}
