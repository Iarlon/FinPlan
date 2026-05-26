using financeiro.Domain.Common;
using financeiro.Infraestructure.Database;
using System.Data;

namespace Financeiro.Infraestructure.Database;

public interface IUnitOfWork : IDisposable
{
    IDbConnection Connection { get; }
    IDbTransaction Transaction { get; }

    Task CommitAsync();
    Task RollbackAsync();
    void TrackEntity(Entity entity);
}
