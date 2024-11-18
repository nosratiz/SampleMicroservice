using FluentAssertions;
using Frontliners.Identity.InfraStructure.Helper;
using Microsoft.AspNetCore.Identity;

namespace Frontliners.Identity.UnitTest;

public class PasswordManagementTests
{
      [Fact]
    public void GetPasswordStrength_Should_Return_Blank_For_Empty_Password()
    {
        PasswordCheck.GetPasswordStrength("").Should().Be(PasswordStrength.Blank);
    }

    [Fact]
    public void GetPasswordStrength_Should_Return_VeryWeak_For_Short_Password()
    {
        PasswordCheck.GetPasswordStrength("1234").Should().Be(PasswordStrength.VeryWeak);
    }

    [Fact]
    public void GetPasswordStrength_Should_Return_Weak_For_Password_With_One_Strong_Condition()
    {
        PasswordCheck.GetPasswordStrength("12345").Should().Be(PasswordStrength.Weak);
    }

    [Fact]
    public void GetPasswordStrength_Should_Return_Medium_For_Password_With_Two_Strong_Conditions()
    {
        PasswordCheck.GetPasswordStrength("12345aA").Should().Be(PasswordStrength.Medium);
    }

    [Fact]
    public void GetPasswordStrength_Should_Return_Strong_For_Password_With_Three_Strong_Conditions()
    {
        PasswordCheck.GetPasswordStrength("12345aA!").Should().Be(PasswordStrength.VeryStrong);
    }

    [Fact]
    public void GetPasswordStrength_Should_Return_VeryStrong_For_Password_With_All_Strong_Conditions()
    {
        PasswordCheck.GetPasswordStrength("12345aA!@").Should().Be(PasswordStrength.VeryStrong);
    }

    [Fact]
    public void IsStrongPassword_Should_Return_True_For_Strong_Password()
    {
        PasswordCheck.IsStrongPassword("StrongPass1!").Should().BeTrue();
    }

    [Fact]
    public void IsStrongPassword_Should_Return_False_For_Weak_Password()
    {
        PasswordCheck.IsStrongPassword("weak").Should().BeFalse();
    }

    [Fact]
    public void IsValidPassword_Should_Return_True_For_Valid_Password()
    {
        var options = new PasswordOptions
        {
            RequiredLength = 8,
            RequiredUniqueChars = 4,
            RequireNonAlphanumeric = true,
            RequireLowercase = true,
            RequireUppercase = true,
            RequireDigit = true
        };

        PasswordCheck.IsValidPassword("Valid1Pass!", options).Should().BeTrue();
    }

    [Fact]
    public void IsValidPassword_Should_Return_False_For_Invalid_Password()
    {
        var options = new PasswordOptions
        {
            RequiredLength = 8,
            RequiredUniqueChars = 4,
            RequireNonAlphanumeric = true,
            RequireLowercase = true,
            RequireUppercase = true,
            RequireDigit = true
        };

        PasswordCheck.IsValidPassword("invalid", options).Should().BeFalse();
    }
}