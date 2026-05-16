using Financeiro.Domain.Enums;
using MediatR;

namespace Financeiro.Application.Command;

public record CriarMovimentacaoCommand(
int OrcamentoId,
int UsuarioId,
decimal Valor,
TipoMovimentacaoEnum Tipo,
string? Descricao,
string? Tag,
DateTime DataMovimentacao,
CategoriaEnum Categoria
) : IRequest<long>;
