using financeiro.Domain.Common;
using Financeiro.Domain.Entities;

namespace financeiro.Domain.Events;

public class MovimentacaoReclassificadaEvent : IDomainEvent
{
    public long MovimentacaoId { get; }
    public int UsuarioId { get; }
    public int CategoriaNovaId { get; }
    public DateTime OccurredOn { get; }

    public MovimentacaoReclassificadaEvent(
        long movimentacaoId,
        int usuarioId,
        int categoriaNovaId)
    {
        MovimentacaoId = movimentacaoId;
        UsuarioId = usuarioId;
        CategoriaNovaId = categoriaNovaId;
        OccurredOn = DateTime.UtcNow;
    }
}
