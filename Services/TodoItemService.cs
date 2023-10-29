using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Exceptions;
using TodoApi.IServices;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly AppDbContext _context;

        public TodoItemService(AppDbContext context)
        {
            _context = context;
        }
        public async Task DeleteTodoItemAsync(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                throw new NotFoundException("The TodoItem with id isn't exist.");
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<TodoItem?> GetTodoItemAsync(int id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task PostTodoItemAsync(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task PutTodoItemAsync(TodoItem todoItem)
        {
            _context.Entry(todoItem).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(todoItem.Id))
                {
                    throw new NotFoundException("The TodoItem with id isn't exist.");
                }
                else
                {
                    throw;
                }
            }
        }
        public bool CheckTodoItems()
        {
            return _context.TodoItems == null;
        }
        private bool TodoItemExists(int id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task PutSeedDataAsync()
        {
            var transaction = _context.Database.BeginTransaction();
            var todoItem = new TodoItem();
            try
            {
                if(await _context.TodoItems.CountAsync() == 500)
                {
                    for (int i = 1; i <= 500; i++)
                    {
                        todoItem = new TodoItem() { Id = i, Name = $"Item-{i}", IsComplete = true };
                        _context.Entry(todoItem).State = EntityState.Modified;
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch(DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to update seed data.");
            }
        }

        public async Task PostSeedDataAsync()
        {
            var transaction = _context.Database.BeginTransaction();
            var todoItem = new TodoItem();
            try
            {
                if(await _context.TodoItems.CountAsync() == 0)
                {
                    for (int i = 1; i <= 500; i++)
                    {
                        todoItem = new TodoItem() { Name = $"Item-{i}", IsComplete = false };
                        _context.TodoItems.Add(todoItem);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to insert seed data.");
            }
        }

        public async Task DeleteSeedDataAsync()
        {
            var transaction = _context.Database.BeginTransaction();
            var todoItem = new TodoItem();
            try
            {
                if(await _context.TodoItems.CountAsync() == 500)
                {
                    for (int i = 1; i <= 500; i += 2)
                    {
                        todoItem = new TodoItem() { Id = i, Name = $"Item-{i}", IsComplete = true };
                        _context.TodoItems.Remove(todoItem);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                } 
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to delete seed data.");
            }
        }
    }
}
