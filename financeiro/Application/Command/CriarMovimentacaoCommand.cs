using Financeiro.Domain.Enums;
using MediatR;

namespace Financeiro.Application.Command;

public record CriarMovimentacaoCommand(
long UsuarioId,
decimal Valor,
TipoMovimentacaoEnum Tipo,
string? Descricao,
string? Tag,
DateTime DataMovimentacao,
int CategoriaId
) : IRequest<long>;
