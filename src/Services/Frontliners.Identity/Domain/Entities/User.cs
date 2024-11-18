using Frontliners.Common.Mongo;
using Frontliners.Identity.Domains.Enum;
using Frontliners.Identity.InfraStructure.Helper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Frontliners.Identity.Domain.Entities;

public sealed class User  : IIdentifiable
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public string Email { get; private set; } = null!;
    public string Password { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public Status Status { get; private set; }
    
    
    public User(string email, string password, string firstName, string lastName)
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = PasswordManagement.HashPass(password);
        FirstName = firstName;
        LastName = lastName;
        Status = Status.Inactive;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void Update(string email, string password, string firstName, string lastName)
    {
        Email = email;
        Password = PasswordManagement.HashPass(password);
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Activate()
    {
        Status = Status.Active;
    }
    
    public void Deactivate()
    {
        Status = Status.Inactive;
    }
    
    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    
    
    
}