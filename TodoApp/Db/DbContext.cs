using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoApp.Models;

namespace TodoApp.Db
{
    public class DbContext
    {
        public IMongoCollection<User> Users { get; }
        public IMongoCollection<Todo> Todos { get; }

        public DbContext(IOptions<DatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            Users = database.GetCollection<User>("UserCollection");
            Todos = database.GetCollection<Todo>("TodoCollection");
        }
    }
}
