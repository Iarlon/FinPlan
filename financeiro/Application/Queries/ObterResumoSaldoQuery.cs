using Financeiro.Application.Response;
using MediatR;

namespace Financeiro.Application.Queries;

public record ObterResumoSaldoQuery(long UsuarioId) : IRequest<ResumoSaldoResponse>;
