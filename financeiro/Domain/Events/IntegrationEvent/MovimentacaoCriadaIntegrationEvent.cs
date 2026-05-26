using Financeiro.Domain.Enums;

namespace Financeiro.Domain.Events.IntegrationEvent;

public record MovimentacaoCriadaIntegrationEvent(
    long MovimentacaoId,
    decimal Valor,
    long OrcamentoId,
    TipoMovimentacaoEnum Tipo
)
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
