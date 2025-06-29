using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;
using TodoApp.Db;
using TodoApp.Dto;
using TodoApp.Models;
using TodoApp.Services.Interfaces;

namespace TodoApp.Services
{
    public class TodoService(DbContext dbContext) : ITodoService
    {
        private readonly DbContext _dbContext = dbContext;

        public async Task<Todo> CreateTodo(CreateTodoRequestDto todo, string userId)
        {
            try
            {
                var newTodo = new Todo
                {
                    Title = todo.Title,
                    Description = todo.Description,
                    UserId = userId
                };
                await _dbContext.Todos.InsertOneAsync(newTodo);
                return newTodo;
            } catch(Exception e)
            {
                throw new Exception("Error creating Todo" + e.Message);
            }
        }

        public async Task DeleteTodo(string todoId, string userId)
        {
            try
            {
                await _dbContext.Todos.DeleteOneAsync(todoId);
            }
            catch (Exception e) { 
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Todo>> GetAllTodos(string userId)
        {
            try
            {
                var filter = Builders<Todo>.Filter.Eq(todo => todo.UserId, userId);
                var todos = await _dbContext.Todos.Find(filter).ToListAsync();
                return todos;
            }
            catch (Exception e) { 
                throw new Exception(e.Message);
            }
        }

        public async Task<Todo?> GetTodo(string todoId, string userId)
        {
            try
            {
                var filter = Builders<Todo>.Filter.Eq(todo => todo.Id, todoId) &
                Builders<Todo>.Filter.Eq(todo => todo.UserId, userId);

                var todo = await _dbContext.Todos.Find(filter).FirstOrDefaultAsync();
                return todo;
            }
            catch (Exception e)
            {
                throw new Exception($"Error retrieving todo: {e.Message}", e);
            }
        }

        public async Task<Todo?> UpdateTodo(string userId, string todoId, Todo newTodo)
        {
            try
            {
                var filter = Builders<Todo>.Filter.Eq(todo => todo.Id, todoId) &
                             Builders<Todo>.Filter.Eq(todo => todo.UserId, userId);

                var existingTodo = await _dbContext.Todos.Find(filter).FirstOrDefaultAsync();

                if (existingTodo == null)
                {
                    return null;
                }

                existingTodo.Title = newTodo.Title;
                existingTodo.Description = newTodo.Description;

                await _dbContext.Todos.ReplaceOneAsync(filter, existingTodo);
                return existingTodo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
