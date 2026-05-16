using financeiro.Infraestructure.Database;
using System.Data;

namespace Financeiro.Infraestructure.Database;

public class UnitOfWork : IUnitOfWork
{
    public IDbConnection Connection { get; }
    public IDbTransaction Transaction { get; private set; }

    public UnitOfWork(IDbConnectionFactory connectionFactory)
    {
        Connection = connectionFactory.CreateConnection();

        Connection.Open();
        Transaction = Connection.BeginTransaction();
    }

    public Task CommitAsync()
    {
        Transaction.Commit();
        Dispose();
        return Task.CompletedTask;
    }

    public Task RollbackAsync()
    {
        Transaction.Rollback();
        Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Transaction?.Dispose();
        Connection?.Dispose();
    }
}
