using Hangfire;
using TodoApi.BackgroundServices;

namespace TodoApi.Configuration
{
    public static class BackgroundServices
    {
        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<AddTodoItemBackgroundService>();
            services.AddHostedService<UpdateTodoItemBackgroundService>();
            services.AddHostedService<DeleteTodoItemBackgroundService>();
            return services;
        }
        public static IServiceCollection AddHangfireService(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("Default")));
            services.AddHangfireServer();
            return services;
        }
    }
}
