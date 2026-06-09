using MediatR;

namespace Financeiro.Application.Queries;

public record ObterSaldoQuery(long UsuarioId) : IRequest<decimal>;
