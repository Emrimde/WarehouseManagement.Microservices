namespace ProductService.Core.Result;

public enum StatusCode
{
    Success = 200,
    BadRequest = 400,
    NotFound = 404,
}

public class Result<T>
{
    public bool isSuccess { get; }
    public string Message { get; }
    public T? Value { get; }
    public StatusCode StatusCode { get; }

    private Result(bool success, T? value, string message, StatusCode statusCode)
    {
        isSuccess = success;
        Value = value;
        Message = message;
        StatusCode = statusCode; 
    }

    public static Result<T> SuccessResult(string? message)
    {
        return new Result<T>(true, default,message!, StatusCode.Success);
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, string.Empty,StatusCode.Success);
    }

    public static Result<T> Failure(string message,StatusCode statusCode)
    {
        return new Result<T>(false, default, message, statusCode);
    }
}
