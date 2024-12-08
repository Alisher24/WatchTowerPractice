namespace WatchTower.Domain.Shared;

public class Result
{
    protected Result(bool isSuccess, ErrorList errorList)
    {
        if (isSuccess && errorList.Any())
            throw new InvalidOperationException();

        if (!isSuccess && !errorList.Any())
            throw new InvalidOperationException();

        ErrorList = errorList;
        IsSuccess = isSuccess;
    }

    public ErrorList ErrorList { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, new ErrorList([]));

    public static implicit operator Result(Error error) => new(false, new ErrorList([error]));

    public static implicit operator Result(ErrorList errorList) => new(false, errorList);
}

public class Result<TValue> : Result
{
    private Result(TValue value, bool isSuccess, ErrorList errorList)
        : base(isSuccess, errorList)
    {
        _value = value;
    }

    private readonly TValue _value;

    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static Result<TValue> Success(TValue value) => new(value, true, new ErrorList([]));

    public static implicit operator Result<TValue>(TValue value) => new(value, true, new ErrorList([]));

    public static implicit operator Result<TValue>(Error error) => new(default!, false, new ErrorList([error]));

    public static implicit operator Result<TValue>(ErrorList errorList) => new(default!, false, errorList);
}