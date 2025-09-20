using ProductMicroservice.Core.Enums;

namespace ProductMicroservice.Core.Results;

public class Result<T>
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public T? Value { get; }
    public StatusCodeEnum StatusCode { get; }

    private Result(bool success, T? value, string message, StatusCodeEnum statusCode)
    {
        IsSuccess = success;
        Value = value;
        Message = message;
        StatusCode = statusCode; 
    }


    public static Result<T> SuccessResult(string? message)
    {
        return new Result<T>(true, default,message!, StatusCodeEnum.Success);
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, string.Empty,StatusCodeEnum.Success);
    }

    public static Result<T> Failure(string message,StatusCodeEnum statusCode)
    {
        return new Result<T>(false, default, message, statusCode);
    }
}
