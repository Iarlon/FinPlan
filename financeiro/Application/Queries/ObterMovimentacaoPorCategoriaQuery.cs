using Financeiro.Application.Response;
using MediatR;

namespace Financeiro.Application.Queries;

public record ObterMovimentacaoPorCategoriaQuery(long UsuarioId) : IRequest<IEnumerable<MovimentacaoPorCategoriaResponse>>;
