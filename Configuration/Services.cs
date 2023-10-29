using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.IServices;
using TodoApi.Services;

namespace TodoApi.Configuration
{
    public static class Services
    {
        public static IServiceCollection AddSqlDbContext(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
            return services;
        }
        public static IServiceCollection RegisterServices(this IServiceCollection services) 
        {
            services.AddScoped<ITodoItemService, TodoItemService>();
            return services;
        }
    }
}
