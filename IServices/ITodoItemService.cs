using TodoApi.Models;

namespace TodoApi.IServices
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync();
        Task<TodoItem?> GetTodoItemAsync(int id);
        Task PutTodoItemAsync(TodoItem todoItem);
        Task PutSeedDataAsync();
        Task PostTodoItemAsync(TodoItem todoItem);
        Task PostSeedDataAsync();
        Task DeleteTodoItemAsync(int id);
        Task DeleteSeedDataAsync();
        bool CheckTodoItems();
    }
}
