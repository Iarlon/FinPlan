using Financeiro.Domain.Enums;

namespace Financeiro.Application.Request;

public class CriarMovimentacaoRequest
{
    public string? Descricao { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataMovimentacao { get; set; }
    public int CategoriaId { get; set; }
    public string? Tag { get; set; }
}
