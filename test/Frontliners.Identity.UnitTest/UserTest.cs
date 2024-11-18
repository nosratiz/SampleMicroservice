using FluentAssertions;
using Frontliners.Identity.Domain.Entities;
using Frontliners.Identity.Domains.Enum;

namespace Frontliners.Identity.UnitTest;

public class UserTest
{
    
    [Fact]
    public void User_Creation_Should_Set_Properties_Correctly()
    {
        var email = "test@example.com";
        var password = "password";
        var firstName = "John";
        var lastName = "Doe";

        var user = new User(email, password, firstName, lastName);

        user.Email.Should().Be(email);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Status.Should().Be(Status.Inactive);
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Update_Should_Update_Properties_Correctly()
    {
        var user = new User("test@example.com", "password", "John", "Doe");
        var newEmail = "new@example.com";
        var newPassword = "newpassword";
        var newFirstName = "Jane";
        var newLastName = "Smith";

        user.Update(newEmail, newPassword, newFirstName, newLastName);

        user.Email.Should().Be(newEmail);
        user.FirstName.Should().Be(newFirstName);
        user.LastName.Should().Be(newLastName);
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Activate_Should_Set_Status_To_Active()
    {
        var user = new User("test@example.com", "password", "John", "Doe");

        user.Activate();

        user.Status.Should().Be(Status.Active);
    }

    [Fact]
    public void Deactivate_Should_Set_Status_To_Inactive()
    {
        var user = new User("test@example.com", "password", "John", "Doe");

        user.Deactivate();

        user.Status.Should().Be(Status.Inactive);
    }

    [Fact]
    public void Delete_Should_Set_IsDeleted_And_DeletedAt()
    {
        var user = new User("test@example.com", "password", "John", "Doe");

        user.Delete();

        user.IsDeleted.Should().BeTrue();
        user.DeletedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}
