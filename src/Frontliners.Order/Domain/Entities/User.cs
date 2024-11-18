using Frontliners.Common.Mongo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Frontliners.Order.Domain.Entities;

public class User : IIdentifiable
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}