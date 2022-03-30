using Microsoft.Extensions.DependencyInjection;
using System;
using TestExamples.ViaCep.Domain.Repositories;
using TestExamples.ViaCep.Repositories;

namespace TestExamples.ViaCep.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddViaCepDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddHttpClient<IAddressRepository, AddressRepository>(c =>
            {
                c.BaseAddress = new Uri("https://viacep.com.br");
            });

            return services;
        }
    }
}
