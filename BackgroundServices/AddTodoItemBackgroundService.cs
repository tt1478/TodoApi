using Microsoft.AspNetCore.Razor.Language.Intermediate;
using TodoApi.IServices;
using TodoApi.Models;

namespace TodoApi.BackgroundServices
{
    public class AddTodoItemBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public AddTodoItemBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var loggerFactory = scope.ServiceProvider.GetService<ILoggerFactory>();
                ILogger? logger = null;
                if(loggerFactory != null)
                {
                    logger = loggerFactory.CreateLogger(nameof(AddTodoItemBackgroundService));
                }
                var todoItemService = scope.ServiceProvider.GetService<ITodoItemService>();
                if (logger != null && todoItemService != null)
                {
                    try
                    {
                        logger.LogInformation("Add todo item service start running at: {time}", DateTimeOffset.Now);
                        await todoItemService.PostSeedDataAsync();
                        logger.LogInformation("Add todo item service stop running at: {time}", DateTimeOffset.Now);
                    }
                    catch (Exception ex)
                    {
                        logger.LogInformation("Add todo item service stop running and there is an exception: {exception}", ex.Message);
                        throw;
                    }
                }
            }
        }
    }
}
