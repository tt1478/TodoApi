using Microsoft.Extensions.Logging;
using TodoApi.IServices;
using TodoApi.Models;

namespace TodoApi.BackgroundServices
{
    public class UpdateTodoItemBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public UpdateTodoItemBackgroundService(IServiceProvider serviceProvider)
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
                    logger = loggerFactory.CreateLogger(nameof(UpdateTodoItemBackgroundService));
                }
                var todoItemService = scope.ServiceProvider.GetService<ITodoItemService>();
                if(logger != null && todoItemService != null)
                {
                    try
                    {
                        Task.Delay(5000).Wait();
                        logger.LogInformation("Update todo item service start running at: {time}", DateTimeOffset.Now);
                        await todoItemService.PutSeedDataAsync();
                        logger.LogInformation("Update todo item service stop running at: {time}", DateTimeOffset.Now);
                    }
                    catch (Exception ex)
                    {
                        logger.LogInformation("Update todo item service stop running and there is and exception: {exception}", ex.Message);
                        throw;
                    }
                    
                }
            }
        }
    }
}
