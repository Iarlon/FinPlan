namespace Financeiro.Application.Response;

public record MovimentacaoRecentesResponse(string Categoria, DateTime DataMovimentacao, string Descricao, decimal Valor, string Tag);
