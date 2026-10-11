namespace NorthWind.Result.Entities;

public class Result<TError>
{
    public TError? ErrorValue { get; private set; }
    public bool HasError { get; private set; }

    public Result() => HasError = false;

    public Result(TError errorValue)
    {
        HasError = true;
        ErrorValue = errorValue;
    }

    public void HandleResult(Action<TError> whenHasErrorAction, Action? whenIsSuccessAction = null)
    {
        if (HasError)
            whenHasErrorAction(ErrorValue!);
        else
            whenIsSuccessAction?.Invoke();
    }
}
