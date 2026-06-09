using Financeiro.Domain.Enums;

namespace Financeiro.Infraestructure.Model;

public record MovimentacaoRecenteModel(
    decimal Valor,
    DateTime DataMovimentacao,
    TipoMovimentacaoEnum TipoMovimentacao,
    string DescricaoMovimentacao,
    string Tag,
    string Categoria)
{
    public MovimentacaoRecenteModel() : this(default, default, default, default, default, default) { }
    public decimal ValorComSinal =>
        TipoMovimentacao == TipoMovimentacaoEnum.despesa ? -Valor : Valor;
}
