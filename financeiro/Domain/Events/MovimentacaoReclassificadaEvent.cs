using financeiro.Domain.Common;
using Financeiro.Domain.Entities;

namespace financeiro.Domain.Events;

public class MovimentacaoReclassificadaEvent : IDomainEvent
{
    public long MovimentacaoId { get; }
    public long UsuarioId { get; }
    public long CategoriaNovaId { get; }
    public DateTime OccurredOn { get; }

    public MovimentacaoReclassificadaEvent(
        long movimentacaoId,
        long usuarioId,
        long categoriaNovaId)
    {
        MovimentacaoId = movimentacaoId;
        UsuarioId = usuarioId;
        CategoriaNovaId = categoriaNovaId;
        OccurredOn = DateTime.UtcNow;
    }
}
