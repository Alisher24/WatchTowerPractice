namespace WatchTower.Domain.Shared;

public record Error
{
    private const string Separator = "||";

    private Error(string code, string message, ErrorType type, string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }

    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    public string? InvalidField { get; } = null;

    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

    public static Error Validation(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Validation, invalidField);
    
    public static Error Failure(string code, string message) =>
        new(code, message, ErrorType.Failure);

    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);

    public static Error AlreadyExists(string code, string message) =>
        new(code, message, ErrorType.AlreadyExists);

    public string Serialize()
    {
        return string.Join(Separator, Code, Message, Type);
    }

    public static Error Deserialize(string serialize)
    {
        var parts = serialize.Split(Separator);

        if (parts.Length < 3)
            throw new ArgumentException("Invalid serialized format");

        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
            throw new ArgumentException("Invalid serialized format");

        return new Error(parts[0], parts[1], type);
    }
}