using Financeiro.Application.Response;
using MediatR;

namespace Financeiro.Application.Command;

public record LoginCommand(
    string Email,
    string Senha)
    : IRequest<LoginResponse>;
