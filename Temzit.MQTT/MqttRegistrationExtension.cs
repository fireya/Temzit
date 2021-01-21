using Microsoft.Extensions.DependencyInjection;

namespace Temzit.MQTT
{
    public static class MqttRegistrationExtension
    {
        public static IServiceCollection AddMqtt(this IServiceCollection services)
        {
            services.AddSingleton<IMqttSender, MqttSender>();
            return services;
        }
    }
}