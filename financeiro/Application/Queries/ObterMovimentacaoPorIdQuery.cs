using Financeiro.Application.Response;
using MediatR;

namespace Financeiro.Application.Queries;

public record ObterMovimentacaoPorIdQuery(long Id) : IRequest<MovimentacaoResponse>;
