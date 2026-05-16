using Financeiro.Domain.Enums;

namespace Financeiro.Application.Request;

public class CriarMovimentacaoRequest
{
    public string? Descricao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataMovimentacao { get; set; }
    public TipoMovimentacaoEnum Tipo { get; set; }
    public CategoriaEnum Categoria { get; set; }
    public string? Tag { get; set; }
}
