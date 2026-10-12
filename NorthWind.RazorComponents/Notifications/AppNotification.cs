namespace NorthWind.RazorComponents.Notifications;

public enum AppNotificationSeverity
{
    Info,
    Warning,
    Error
}

public sealed record AppNotification(
    string Title,
    string Message,
    string Solution,
    AppNotificationSeverity Severity = AppNotificationSeverity.Error);
