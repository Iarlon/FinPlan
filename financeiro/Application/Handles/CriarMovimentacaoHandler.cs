using Financeiro.Domain.Repository;
using MediatR;
using Financeiro.Application.Command;
using financeiro.Domain.Repository;
using Financeiro.Domain.Entities;
using Financeiro.Application.Exceptions;
using Financeiro.Infraestructure.Database;

namespace Financeiro.Application.Handles;

public class CriarMovimentacaoHandler
: IRequestHandler<CriarMovimentacaoCommand, long>
{
    private readonly IMovimentacaoRepository _movimentacaoRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ICategoriaRepository _categoriaRepo;
    private readonly IOrcamentoRepository _orcamentoRepo;
    private readonly IUnitOfWork _uow;

    public CriarMovimentacaoHandler(
        IUnitOfWork uow,
        IMovimentacaoRepository movimentacaoRepo,
        IUsuarioRepository usuarioRepo,
        ICategoriaRepository categoriaRepo,
        IOrcamentoRepository orcamentoRepo)
    {
        _uow = uow;
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

        var categoria = await _categoriaRepo.ObterCategoriaPorId(request.CategoriaId);
        if (categoria is null)
            throw new NotFoundException("Categoria", request.CategoriaId);

        var orcamento =
            await _orcamentoRepo.ObterOuCriarOrcamentoId(request.UsuarioId);

        var movimentacao = Movimentacao.CriarMovimentacao(
            request.Valor,
            categoria,
            orcamento,
            request.UsuarioId,
            request.Descricao,
            request.Tag
            );

        await _movimentacaoRepo.AdicionarMovimentacao(movimentacao);
        await _uow.CommitAsync();

        return movimentacao.Id;
    }
}
