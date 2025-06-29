using TodoApp.Dto;
using TodoApp.Models;

namespace TodoApp.Services.Interfaces
{
    public interface ITodoService
    {
        public Task<Todo> CreateTodo(CreateTodoRequestDto todo, string userId);
        public Task DeleteTodo(string todoId, string userId);
        public Task<Todo?> GetTodo(string todoId, string userId);
        public Task<List<Todo>> GetAllTodos(string userId);
        public Task<Todo?> UpdateTodo(string userId, string todoId, Todo newTodo);
    }
}
