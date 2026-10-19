namespace NorthWind.Sales.Frontend.Views.Pages;

public partial class CreateOrder
{
    [Inject]
    public CreateOrderViewModel ViewModel { get; set; } = default!;

    private ErrorBoundary? ErrorBoundaryRef;

    private void Recover() => ErrorBoundaryRef?.Recover();
}
