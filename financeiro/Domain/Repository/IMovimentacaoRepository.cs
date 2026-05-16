using Financeiro.Domain.Entities;

namespace Financeiro.Domain.Repository;

public interface IMovimentacaoRepository
{
    Task AdicionarMovimentacao(Movimentacao movimentacao);
    Task<Movimentacao> ObterMovimentacaoPorId(long id);
    Task AtualizarMovimentacao(Movimentacao movimentacao);
}
