using ProductMicroservice.Core.Enums;

namespace ProductMicroservice.Core.Results;
public class Result
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public StatusCodeEnum StatusCode { get; set; }

    private Result(bool isSuccess, string message, StatusCodeEnum statusCode)
    {
        IsSuccess = isSuccess;
        Message = message;
        StatusCode = statusCode;
    }

    private Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public static Result Failure(string message, StatusCodeEnum statusCode)
    {
        return new Result(false, message, statusCode);
    }

    public static Result Success()
    {
        return new Result(true);
    }
}
