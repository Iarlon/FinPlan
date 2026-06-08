using Financeiro.Domain.Enums;
using MediatR;

namespace Financeiro.Application.Command;

public record CriarMovimentacaoCommand(
long UsuarioId,
decimal Valor,
string? Descricao,
string? Tag,
DateTime DataMovimentacao,
int CategoriaId
) : IRequest<long>;
