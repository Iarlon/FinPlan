using Financeiro.Domain.Enums;

namespace Financeiro.Infraestructure.Model;

public record MovimentacaoHistoricoModel(decimal Valor, DateTime DataMovimentacao, string Categoria, string Descricao, TipoMovimentacaoEnum TipoMovimentacao, string Tag)
{
    public MovimentacaoHistoricoModel() : this(default, default, default, default, default, default) { }
    public decimal ValorComSinal =>
        TipoMovimentacao == TipoMovimentacaoEnum.despesa ? -Valor : Valor;
}
