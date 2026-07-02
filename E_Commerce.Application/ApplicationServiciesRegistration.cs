using E_Commerce.Application.Contracts;
using E_Commerce.Application.Servicies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application
{
    public static class ApplicationServiciesRegistration
    {
        public static IServiceCollection ApplicationServicesRegistration ( this IServiceCollection services)
        {
            services.AddAutoMapper(c => { },typeof(ApplicationServiciesRegistration).Assembly);
            services.AddScoped<IProductService, ProductService>();
            return services;
        }

    }
}
