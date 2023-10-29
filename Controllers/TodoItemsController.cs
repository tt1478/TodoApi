using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Exceptions;
using TodoApi.IServices;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        public TodoItemsController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            if (_todoItemService.CheckTodoItems())
            {
                return NotFound();
            }
            var result = await _todoItemService.GetTodoItemsAsync();
            return result.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            if (_todoItemService.CheckTodoItems())
            {
                return NotFound();
            }
            var todoItem = await _todoItemService.GetTodoItemAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }
            return todoItem;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }
            try
            {
                await _todoItemService.PutTodoItemAsync(todoItem);
            }
            catch(NotFoundException)
            {
                return NotFound();
            }
            catch (Exception) 
            {
                throw;
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            if (_todoItemService.CheckTodoItems())
            {
                return Problem("Entity set 'AppDbContext.TodoItems'  is null.");
            }
            await _todoItemService.PostTodoItemAsync(todoItem);
            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            if (_todoItemService.CheckTodoItems())
            {
                return NotFound();
            }
            try
            {
                await _todoItemService.DeleteTodoItemAsync(id);
            }
            catch(NotFoundException)
            {
                return NotFound();
            } 
            catch(Exception)
            {
                throw;
            }
            return NoContent();
        }
    }
}
