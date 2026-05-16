using Financeiro.Domain.Entities;

namespace Financeiro.Domain.Repository;

public interface IOrcamentoRepository
{
    Task<Orcamento> ObterOrcamentoPorId(int id);
    Task AtualizarOrcamento(Orcamento orcamento);
}
