using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.IServices;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireJobsController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        public HangfireJobsController(ITodoItemService todoItemService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _todoItemService = todoItemService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;

        }
        [HttpGet("FireAndForgetJob")]
        public ActionResult CreateFireAndForgetJob()
        {
            var todoItem = new TodoItem() { Name = "Item-501", IsComplete = false };
            _backgroundJobClient.Enqueue(() => _todoItemService.PostTodoItemAsync(todoItem));
            return Ok();
        }
        [HttpGet("DelayedJob")]
        public ActionResult CreateDelayedJob()
        {
            var todoItem = new TodoItem() { Id = 2, Name = "Item-2", IsComplete = false };
            _backgroundJobClient.Schedule(() => _todoItemService.PutTodoItemAsync(todoItem), TimeSpan.FromSeconds(30));
            return Ok();
        }
        [HttpGet("ReccuringJob")]
        public ActionResult CreateReccuringJob()
        {
            var todoItem = new TodoItem() { Name = "Item-1000", IsComplete = false };
            _recurringJobManager.AddOrUpdate("jobId", () => _todoItemService.PostTodoItemAsync(todoItem), Cron.Minutely);
            return Ok();
        }
        [HttpGet("ContinuationJob")]
        public ActionResult CreateContinuationJob()
        {
            var todoItem = new TodoItem() { Id = 2, Name = "Item-502", IsComplete = false };
            var parentJobId = _backgroundJobClient.Enqueue(() => _todoItemService.PutTodoItemAsync(todoItem));
            todoItem = new TodoItem() { Id = 2, Name = "Item-503", IsComplete = false };
            _backgroundJobClient.ContinueJobWith(parentJobId, () => _todoItemService.PutTodoItemAsync(todoItem));
            return Ok();
        }
    }
}
