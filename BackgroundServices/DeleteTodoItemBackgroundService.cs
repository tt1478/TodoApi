using TodoApi.IServices;
using TodoApi.Models;

namespace TodoApi.BackgroundServices
{
    public class DeleteTodoItemBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public DeleteTodoItemBackgroundService(IServiceProvider serviceProvider)
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
                if (logger != null && todoItemService != null)
                {
                    try
                    {
                        Task.Delay(10000).Wait();
                        logger.LogInformation("Delete todo item service start running at: {time}", DateTimeOffset.Now);
                        await todoItemService.DeleteSeedDataAsync();
                        logger.LogInformation("Delete todo item service stop running at: {time}", DateTimeOffset.Now);
                    }
                    catch (Exception ex) 
                    {
                        logger.LogInformation("Delete todo item service stop running and there is an exception: {exception}", ex.Message);
                        throw;
                    }
                }
            }
        }
    }
}
