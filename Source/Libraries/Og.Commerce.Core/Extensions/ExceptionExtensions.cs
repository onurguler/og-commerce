namespace Og.Commerce.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static Result.Result ToResult(this Exception exception) 
        {
            return Result.Result.Error(exception.Message);
        }
    }

    public static class ResultExecuter
    {
        public static Result.Result<T> Exec<T>(Func<T> func)
        {
            try
            {
                var returnVal = func();
                return Result.Result.Success(returnVal);
            }
            catch (Exception ex)
            {
                return ex.ToResult();
            }
        }

        public static Result.Result Exec(Action func)
        {
            try
            {
                func();
                return Result.Result.Success();
            }
            catch (Exception ex)
            {
                return ex.ToResult();
            }
        }

        public static async Task<Result.Result<T>> ExecAsync<T>(Func<Task<T>> func)
        {
            try
            {
                var returnVal = await func();
                return Result.Result.Success(returnVal);
            }
            catch (Exception ex)
            {
                return ex.ToResult();
            }
        }

        public static async Task<Result.Result> ExecAsync(Func<Task> func)
        {
            try
            {
                await func();
                return Result.Result.Success();
            }
            catch (Exception ex)
            {
                return ex.ToResult();
            }
        }
    }
}
