using TodoApp.Models;

namespace TodoApp.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> CreateUser(User user);
        public Task<User> GetUser(string id);
        public Task<User> UpdateUser(string id, User user);
        public Task DeleteUser(string id);
        public Task<User> GetUserByEmail(string email);
        public Task<List<User>> GetUsers();
        public Task SaveToken(string id, string token);
    }
}
