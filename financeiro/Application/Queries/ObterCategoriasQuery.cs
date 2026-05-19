using Financeiro.Application.Response;
using MediatR;

namespace Financeiro.Application.Queries;

public record ObterCategoriasQuery : IRequest<IEnumerable<CategoriaResponse>>;
