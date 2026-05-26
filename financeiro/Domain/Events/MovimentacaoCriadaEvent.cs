using financeiro.Domain.Common;
using Financeiro.Domain.Entities;

namespace financeiro.Domain.Events;

public class MovimentacaoCriadaEvent : IDomainEvent
{
    public DateTime OccurredOn { get; private set; }
    public Movimentacao Movimentacao { get; }

    public MovimentacaoCriadaEvent(Movimentacao movimentacao)
    {
        Movimentacao = movimentacao;
        OccurredOn = DateTime.UtcNow;
    }
}
