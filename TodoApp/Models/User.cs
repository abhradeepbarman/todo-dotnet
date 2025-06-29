using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApp.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
