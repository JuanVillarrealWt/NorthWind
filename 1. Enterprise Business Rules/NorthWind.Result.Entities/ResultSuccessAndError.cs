namespace NorthWind.Result.Entities;

public class Result<TSuccess, TError> : Result<TError>
{
    public TSuccess? SuccessValue { get; private set; }

    public Result(TSuccess successValue) : base() => SuccessValue = successValue;

    public Result(TError errorValue) : base(errorValue) { }

    public void HandleResult(Action<TSuccess> whenIsSuccessAction, Action<TError> whenHasErrorAction)
    {
        if (HasError)
            whenHasErrorAction(ErrorValue!);
        else
            whenIsSuccessAction(SuccessValue!);
    }
}
