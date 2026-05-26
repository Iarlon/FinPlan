using Financeiro.Application.Queries;
using Financeiro.Application.Response;
using Financeiro.Domain.Enums;
using Financeiro.Domain.Repository;
using MediatR;

namespace Financeiro.Application.Handles;

public class ObterCategoriasHandler : IRequestHandler<ObterCategoriasQuery, IEnumerable<CategoriaResponse>>
{
    private readonly ICategoriaRepository _repository;

    public ObterCategoriasHandler(ICategoriaRepository repository) => _repository = repository;
    public async Task<IEnumerable<CategoriaResponse>> Handle(ObterCategoriasQuery request, CancellationToken cancellationToken)
    {
        var categorias = await _repository.ObterCategorias();
        return categorias.Select(c => new CategoriaResponse(c.Descricao, (TipoMovimentacaoEnum)c.Tipo));
    }

}
