using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace OrderManagement.Domain.Shared.ValueObjects;

public record struct Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string value)
    {
        if (!IsFormatValid(value, TimeSpan.FromMilliseconds(250)))
            return Result.Failure<Email>("Email format is invalid");

        return new Email(value);
    }
    
    private const string RegexPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    
    private static bool IsFormatValid(string email, TimeSpan validationTimeout)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        try
        {
            return Regex.IsMatch(email,
                                 RegexPattern,
                                 RegexOptions.IgnoreCase,
                                 validationTimeout);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}