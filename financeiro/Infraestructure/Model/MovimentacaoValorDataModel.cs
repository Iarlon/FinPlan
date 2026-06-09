using Financeiro.Domain.Enums;

namespace Financeiro.Infraestructure.Model;

public record MovimentacaoValorDataModel(
    decimal Valor,
    DateTime DataMovimentacao,
    TipoMovimentacaoEnum TipoMovimentacao)
{
    public MovimentacaoValorDataModel() : this(default, default, default) { }
    public decimal ValorComSinal =>
        TipoMovimentacao == TipoMovimentacaoEnum.despesa ? -Valor : Valor;
}
