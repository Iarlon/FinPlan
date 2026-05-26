using Financeiro.Domain.Entities;
using Financeiro.Infraestructure.Model;

namespace Financeiro.Domain.Repository;

public interface IMovimentacaoRepository
{
    Task AdicionarMovimentacao(Movimentacao movimentacao);
    Task<IEnumerable<MovimentacaoValorECategoriaModel>> ObterValorECategoria(long usuarioId);
    Task<Movimentacao> ObterMovimentacaoPorId(long id);
    Task AtualizarMovimentacao(Movimentacao movimentacao);
    Task<IEnumerable<MovimentacaoValorDataModel>> ObterMovimentacaoPorPeriodo(long usuarioId, DateTime inicio, DateTime fim);
}
