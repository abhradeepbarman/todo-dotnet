using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApp.Dto;
using TodoApp.Models;
using TodoApp.Services.Interfaces;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateTodo([FromBody] CreateTodoRequestDto createTodoRequestDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token");
            }

            var todo = await _todoService.CreateTodo(createTodoRequestDto, userId);
            return Ok(todo);
        }

        [HttpDelete("{todoId}")]
        [Authorize]
        public async Task<ActionResult> DeleteTodo(string todoId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token");
            }

            if (string.IsNullOrEmpty(todoId))
            {
                return BadRequest();
            }

            var todo = await _todoService.GetTodo(todoId, userId);
            if (todo == null)
            {
                return NotFound();
            }

            if (todo.UserId != userId)
            {
                return Unauthorized();
            }

            await _todoService.DeleteTodo(todoId, userId);
            return Ok(todo);
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<List<Todo>>> GetAllTodos()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token");
            }

            var todos = await _todoService.GetAllTodos(userId);
            return todos;
        }

        [HttpPut("{todoId}")]
        [Authorize]
        public async Task<ActionResult> UpdateTodo(string todoId, UpdateTodoResponseDto dto) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token");
            }

            if (string.IsNullOrEmpty(todoId))
            {
                return BadRequest();
            }

            var todo = await _todoService.GetTodo(todoId, userId);
            if (todo == null)
            {
                return NotFound();
            }

            if (todo.UserId != userId)
            {
                return Unauthorized();
            }

            var updatedTodo = new Todo
            {
                Id = todoId,
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description,
            };

            await _todoService.UpdateTodo(userId, todoId, updatedTodo);
            return Ok(updatedTodo);
        }
    }
}
