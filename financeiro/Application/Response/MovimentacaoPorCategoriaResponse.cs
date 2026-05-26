using Financeiro.Domain.Enums;

namespace Financeiro.Application.Response;

public record MovimentacaoPorCategoriaResponse(string Categoria, decimal Valor, TipoMovimentacaoEnum Tipo, string DescricaoMovimentacao);
