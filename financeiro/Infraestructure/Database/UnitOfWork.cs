using financeiro.Application.Contract;
using financeiro.Domain.Common;
using financeiro.Infraestructure.Database;
using Microsoft.AspNetCore.Components;
using System.Data;

namespace Financeiro.Infraestructure.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDomainEventDispatcher _dispatcher;

    private readonly HashSet<Entity> _trackedEntities = new();
    public IDbConnection Connection { get; }
    public IDbTransaction Transaction { get; private set; }

    public UnitOfWork(IDbConnectionFactory connectionFactory, IDomainEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        Connection = connectionFactory.CreateConnection();

        Connection.Open();
        Transaction = Connection.BeginTransaction();
    }
    public void TrackEntity(Entity entity)
    {
        if (entity != null)
        {
            _trackedEntities.Add(entity);
        }
    }

    public async Task CommitAsync()
    {
        try
        { 
            await DispatchEventsAsync();
            Transaction.Commit();
        }
        catch
        {
            Transaction.Rollback();
            throw;
        }
        finally
        {
            Dispose();
        }
    }

    public Task RollbackAsync()
    {
        Transaction.Rollback();
        Dispose();
        return Task.CompletedTask;
    }
    private async Task DispatchEventsAsync()
    {
        var domainEvents = _trackedEntities
            .SelectMany(x => x.DomainEvents)
            .ToList();
        foreach (var entity in _trackedEntities)
        {
            entity.ClearDomainEvents();
        }
        if (domainEvents.Any())
        {
            await _dispatcher.DispatchAsync(domainEvents);
        }
        _trackedEntities.Clear();
    }
    public void Dispose()
    {
        Transaction?.Dispose();
        Connection?.Dispose();
    }
}
