using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApp.Models
{
    public class Todo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public required string UserId { get; set; }
    }
}
