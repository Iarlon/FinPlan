using Financeiro.Domain.Enums;

namespace Financeiro.Application.Response;

public record MovimentacaoHistoricoResponse(string Categoria, DateTime DataMovimentacao, string Descricao, decimal Valor, TipoMovimentacaoEnum TipoMovimentacao, string Tag);
