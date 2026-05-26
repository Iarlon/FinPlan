namespace Financeiro.Application.Response;

public record MovimentacaoResponse(
    long Id,
    decimal Valor,
    string Tipo,
    string? Descricao,
    string? Categoria,
    DateTime DataMovimentacao
    );
