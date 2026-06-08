using Financeiro.Domain.Enums;

namespace Financeiro.Infraestructure.Model;

public record MovimentacaoValorECategoriaModel(
    string Categoria,
    string DescricaoMovimentacao,
    decimal Valor,
    TipoMovimentacaoEnum TipoMovimentacao)
{
    public MovimentacaoValorECategoriaModel() : this(default, default, default, default) { }
    public decimal ValorComSinal =>
        TipoMovimentacao == TipoMovimentacaoEnum.despesa ? -Valor : Valor;
}
