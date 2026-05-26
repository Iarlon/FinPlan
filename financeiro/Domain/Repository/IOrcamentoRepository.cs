using Financeiro.Domain.Entities;

namespace Financeiro.Domain.Repository;

public interface IOrcamentoRepository
{
    Task<Orcamento> ObterOrcamentoPorId(long id);
    Task AtualizarOrcamento(Orcamento orcamento);
    Task<Orcamento> ObterSaldoPorUsuarioId(long usuarioId);
    Task<long> ObterOuCriarOrcamentoId(long usuarioId);
}
