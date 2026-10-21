using System.Transactions;

namespace NorthWind.Transactions.Entities.Services;

internal sealed class DomainTransaction : IDomainTransaction, IDisposable
{
    private TransactionScope? transactionScope;
    private bool isActive;

    public void BeginTransaction()
    {
        if (isActive || transactionScope is not null)
            throw new InvalidOperationException("A domain transaction is already active.");

        isActive = true;

        if (OperatingSystem.IsWindows())
        {
            TransactionManager.ImplicitDistributedTransactions = true;
            transactionScope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);
        }
    }

    public void CommitTransaction()
    {
        if (!isActive)
            throw new InvalidOperationException("No domain transaction is active.");

        if (OperatingSystem.IsWindows())
        {
            transactionScope?.Complete();
        }

        Dispose();
    }

    public void RollbackTransaction() => Dispose();

    public void Dispose()
    {
        transactionScope?.Dispose();
        transactionScope = null;
        isActive = false;
    }
}
