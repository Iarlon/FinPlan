using Financeiro.Domain.Enums;

namespace Financeiro.Application.Response;

public record CategoriaResponse(string descricao, TipoMovimentacaoEnum tipo);
