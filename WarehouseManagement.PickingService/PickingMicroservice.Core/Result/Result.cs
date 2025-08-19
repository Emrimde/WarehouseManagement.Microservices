namespace PickingMicroservice.Core.Result;
public enum StatusCode
{
    Success = 200,
    BadRequest = 400,
    NotFound = 404,
    Created = 201,
    NoContent = 204,
    Conflict = 409,
}

public class Result<T>
{
    public T? Value { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public StatusCode StatusCode { get; set; }

    private Result(bool success, T? value, string message, StatusCode statusCode)
    {
        IsSuccess = success;
        Value = value;
        Message = message;
        StatusCode = statusCode;
    }
    public static Result<T> SuccessResult(string? message)
    {
        return new Result<T>(true, default, message!, StatusCode.Success);
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, string.Empty, StatusCode.Success);
    }

    public static Result<T> Failure(string message, StatusCode statusCode)
    {
        return new Result<T>(false, default, message, statusCode);
    }

}
