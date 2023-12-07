namespace Solvintech.Shared.Utils;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);
}

public class Result<T> : Result
{
    private readonly T? _value;

    private Result(T value) : base(true, Error.None)
    {
        _value = value;
    }

    private Result(Error error) : base(false, error)
    {
        _value = default;
    }

    public T Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException("Cannot access the value of a failed result.");
            }
            return _value!;
        }
    }

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Failure(Error error, List<string> details = null)
    {
        error.AddDetails(details);
        return new Result<T>(error);
    }

    public static Result<T> Failure(Error error, string details)
    {
        error.AddDetails(new List<string> { details });
        return new Result<T>(error);
    }
}

public static class ResultExtensions
{
    public static T Match<T, TV>(
        this Result<TV> result,
        Func<TV, T> onSuccess,
        Func<Error, T> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
    }
}