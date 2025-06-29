using MongoDB.Driver;
using TodoApp.Db;
using TodoApp.Models;
using TodoApp.Services.Interfaces;

namespace TodoApp.Services
{
    public class UserService : IUserService
    {
        public readonly DbContext _dbContext;

        public UserService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUser(User user)
        {
            try
            {
                await _dbContext.Users.InsertOneAsync(user);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating user", ex);
            }
        }

        public async Task DeleteUser(string id)
        {
            try
            {
                await _dbContext.Users.DeleteOneAsync(user => user.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting user", ex);
            }
        }

        public async Task<User> GetUser(string id)
        {
            try
            {
                var cursor = await _dbContext.Users.FindAsync(user => user.Id == id);
                return await cursor.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user", ex);
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                var cursor = await _dbContext.Users.FindAsync(user => user.Email == email);
                return await cursor.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user by email", ex);
            }
        }

        public async Task<List<User>> GetUsers()
        {
            try
            {
                var cursor = await _dbContext.Users.FindAsync(_ => true);
                return await cursor.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users", ex);
            }
        }

        public async Task<User> UpdateUser(string id, User user)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, id);
                await _dbContext.Users.ReplaceOneAsync(filter, user);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user", ex);
            }
        }

        public async Task SaveToken(string id, string token)
        {
            try
            {
                await _dbContext.Users.FindOneAndUpdateAsync(
                    user => user.Id == id,
                    Builders<User>.Update.Set(u => u.Token, token)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving token", ex);
            }
        }
    }
}
