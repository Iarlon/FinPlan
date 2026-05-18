using Financeiro.Domain.Repository;
using MediatR;
using Financeiro.Application.Command;
using financeiro.Domain.Repository;
using Financeiro.Domain.Entities;
using Financeiro.Application.Exceptions;

namespace Financeiro.Application.Handles;

public class CriarMovimentacaoHandler
: IRequestHandler<CriarMovimentacaoCommand, long>
{
    private readonly IMovimentacaoRepository _movimentacaoRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ICategoriaRepository _categoriaRepo;
    private readonly IOrcamentoRepository _orcamentoRepo;

    public CriarMovimentacaoHandler(
        IMovimentacaoRepository movimentacaoRepo,
        IUsuarioRepository usuarioRepo,
        ICategoriaRepository categoriaRepo,
        IOrcamentoRepository orcamentoRepo)
    {
        _movimentacaoRepo = movimentacaoRepo;
        _usuarioRepo = usuarioRepo;
        _categoriaRepo = categoriaRepo;
        _orcamentoRepo = orcamentoRepo;
    }

    public async Task<long> Handle(CriarMovimentacaoCommand request, CancellationToken ct)
    {
        var usuario = await _usuarioRepo.ObterUsuarioPorId(request.UsuarioId);
        if (usuario is null)
            throw new NotFoundException("Usuario", request.UsuarioId);

        var categoria = await _categoriaRepo.ObterCategoriaPorId((int)request.Categoria);
        if (categoria is null)
            throw new NotFoundException("Categoria", request.Categoria);
        var movimentacao = Movimentacao.CriarMovimentacao(
            command.Valor,
            command.Categoria,
            command.OrcamentoId,
            command.UsuarioId,
            command.Tipo,
            command.Tag,
            command.Descricao
            );

        if (!string.IsNullOrWhiteSpace(command.Descricao))
            movimentacao.AlterarDescricao(command.Descricao);

        if (!string.IsNullOrWhiteSpace(command.Tag))
            movimentacao.AlterarTag(command.Tag);

        await _movimentacaoRepo.AdicionarMovimentacao(movimentacao);

        return movimentacao.Id;
    }
}
