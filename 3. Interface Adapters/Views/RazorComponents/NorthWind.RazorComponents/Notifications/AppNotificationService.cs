namespace NorthWind.RazorComponents.Notifications;

public sealed class AppNotificationService
{
    public event Action<AppNotification>? NotificationRaised;

    public void ShowError(string title, string message, string solution) =>
        NotificationRaised?.Invoke(new AppNotification(title, message, solution, AppNotificationSeverity.Error));

    public void ShowWarning(string title, string message, string solution) =>
        NotificationRaised?.Invoke(new AppNotification(title, message, solution, AppNotificationSeverity.Warning));

    public void ShowInfo(string title, string message, string solution) =>
        NotificationRaised?.Invoke(new AppNotification(title, message, solution, AppNotificationSeverity.Info));
}
