using MediatR;

namespace Financeiro.Application.Command;

public record CriarUsuarioCommand(string Nome, string Email, string Senha) : IRequest<long>;
