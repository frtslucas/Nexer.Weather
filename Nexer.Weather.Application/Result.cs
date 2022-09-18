namespace Nexer.Weather.Application
{
    public class Result<T>
    {
        public bool IsSuccess { get; init; }
        public string ErrorMessage { get; init; }
        public T Content { get; init; }

        private Result(bool isSuccess, string errorMessage = "", T content = default)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Content = content;  
        }

        public static Result<T> Success(T content) => new(isSuccess: true, content: content);
        public static Result<T> Error(string errorMessage) => new(isSuccess: false, errorMessage: errorMessage);
    }
}
