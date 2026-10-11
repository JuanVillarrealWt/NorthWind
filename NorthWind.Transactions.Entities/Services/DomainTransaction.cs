using System.Transactions;

namespace NorthWind.Transactions.Entities.Services;

internal sealed class DomainTransaction : IDomainTransaction, IDisposable
{
    private TransactionScope? transactionScope;

    public void BeginTransaction()
    {
        if (transactionScope is not null)
            throw new InvalidOperationException("A domain transaction is already active.");
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("Distributed domain transactions require Windows.");

        TransactionManager.ImplicitDistributedTransactions = true;
        transactionScope = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);
    }

    public void CommitTransaction()
    {
        if (transactionScope is null)
            throw new InvalidOperationException("No domain transaction is active.");

        transactionScope.Complete();
        Dispose();
    }

    public void RollbackTransaction() => Dispose();

    public void Dispose()
    {
        transactionScope?.Dispose();
        transactionScope = null;
    }
}
