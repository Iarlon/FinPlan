using Financeiro.Application.Response;
using MediatR;

namespace Financeiro.Application.Queries;

public record ObterHistoricoMovimentacaoQuery(
    long UsuarioId,
    int PageNumber,
    int PageSize,
    long? CategoriaId,
    DateTime DataInicio,
    DateTime DataFim,
    int? TipoMovimentacao,
    string? Tag) : IRequest<PagedResponse<MovimentacaoHistoricoResponse>>;
