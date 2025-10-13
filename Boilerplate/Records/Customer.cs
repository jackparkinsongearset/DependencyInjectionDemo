namespace Boilerplate.Records;

public record Customer(Guid CustomerId, string FirstName, string LastName, string? Email)
{
    public override string ToString() => $"{FirstName}__{LastName}";
};