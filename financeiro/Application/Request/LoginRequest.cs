namespace Financeiro.Application.Request;

public record LoginRequest(
    string Email,
    string Senha);
