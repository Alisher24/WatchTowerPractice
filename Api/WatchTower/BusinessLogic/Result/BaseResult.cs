namespace WatchTower.Server.BusinessLogic.Result
{
    public class BaseResult
    {
        public bool IsSuccess => ErrorMessage == null;

        public string ErrorMessage { get; set; }

    }

    public class BaseResult<T> : BaseResult
    {
        public BaseResult(string errorMessage, T data)
        {
            ErrorMessage = errorMessage;
            Data = data;
        }

        public BaseResult() { }

        public T Data { get; set; }
    }
}
