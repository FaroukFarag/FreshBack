namespace FreshBack.Application.Dtos.Shared;

public class ResultDto<T>
{
    public bool Succeeded { get; set; }
    public string Message { get; set; } = default!;
    public T ResultData { get; set; } = default!;

    public static ResultDto<T> CreateSuccessResult(T resultData)
    {
        return new ResultDto<T>
        {
            Succeeded = true,
            ResultData = resultData
        };
    }

    public static ResultDto<T> CreateFailResult(string message)
    {
        return new ResultDto<T>
        {
            Succeeded = false,
            Message = message
        };
    }
}
