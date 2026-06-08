using Financeiro.Application.Queries;
using Financeiro.Application.Response;
using Financeiro.Domain.Repository;
using MediatR;

namespace Financeiro.Application.Handles;

public class ObterMovimentacoesPorCategoriaHandler
    : IRequestHandler<ObterMovimentacaoPorCategoriaQuery, IEnumerable<MovimentacaoPorCategoriaResponse>>
{
    private readonly IMovimentacaoRepository _repository;
    public ObterMovimentacoesPorCategoriaHandler (IMovimentacaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MovimentacaoPorCategoriaResponse>> Handle(
        ObterMovimentacaoPorCategoriaQuery request,
        CancellationToken cancellationToken)
    {
        var mov = await _repository.ObterValorECategoria(request.UsuarioId);
        return mov
        .GroupBy(m => m.Categoria)
        .Select(grupo => new MovimentacaoPorCategoriaResponse(
            grupo.Key,
            grupo.Sum(x => x.ValorComSinal)
        ));
    }
}
