using Microsoft.Extensions.DependencyInjection;

namespace Temzit.Api
{
    public static class TemzitRegistrationExtensions
    {
        public static IServiceCollection AddTemzit(this IServiceCollection services)
        {
            services.AddSingleton<ITemzitReader, TemzitReader>();
            return services;
        }
    }
}