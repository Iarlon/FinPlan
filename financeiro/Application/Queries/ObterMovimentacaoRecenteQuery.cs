using Financeiro.Application.Response;
using MediatR;

namespace Financeiro.Application.Queries;

public record ObterMovimentacaoRecenteQuery(long UsuarioId) : IRequest<IEnumerable<MovimentacaoRecentesResponse>>;
