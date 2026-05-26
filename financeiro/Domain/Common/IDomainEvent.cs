using MediatR;

namespace financeiro.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
