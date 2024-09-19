using FluentAssertions;
using OrderManagement.Domain.Shared.ValueObjects;

namespace OrderManagement.UnitTests.Domain.Shared.ValueObjects;

public class EmailTest
{
    [Fact]
    [Trait("Scenario", "Success")]
    public void EmailCreatedWhenValidEmailIsProvided()
    {
        // Arrange
        string email = "test@test.com";
        
        // Act
        var result = Email.Create(email);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value
              .Value.Should().Be(email);
    }
    
    [Theory]
    [Trait("Scenario", "Failure")]
    [InlineData("test@any")]
    [InlineData("test@")]
    [InlineData("@test")]
    [InlineData("@test@")]
    [InlineData("test@test.com@")]
    [InlineData("@test@test.com")]
    [InlineData(" ")]
    [InlineData("")]
    [InlineData("email")]
    public void EmailCreationShouldResultInFailureWhenInvalidEmailIsProvided(string email)
    {
        // Arrange
        // ...
        
        // Act
        var result = Email.Create(email);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Email format is invalid");
    }
}