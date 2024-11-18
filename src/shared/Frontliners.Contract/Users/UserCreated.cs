namespace Frontliners.Contract.Users;

public record UserCreated(Guid UserId, string Email, string FirstName, string LastName);