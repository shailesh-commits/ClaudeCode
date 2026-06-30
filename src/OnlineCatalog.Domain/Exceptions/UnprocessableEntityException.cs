namespace OnlineCatalog.Domain.Exceptions;

public class UnprocessableEntityException : Exception
{
    public string Field { get; }

    public UnprocessableEntityException(string field, string message) : base(message)
    {
        Field = field;
    }
}
