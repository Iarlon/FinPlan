using Financeiro.Application.Queries;
using Financeiro.Application.Response;
using Financeiro.Domain.Repository;
using MediatR;

namespace Financeiro.Application.Handles;

public class ObterHistoricoMovimentacoesHandler
    : IRequestHandler<ObterHistoricoMovimentacaoQuery, PagedResponse<MovimentacaoHistoricoResponse>>
{
    private readonly IMovimentacaoRepository _repository;

    public ObterHistoricoMovimentacoesHandler(IMovimentacaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<MovimentacaoHistoricoResponse>> Handle(
        ObterHistoricoMovimentacaoQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _repository.ObterHistoricoPaginado(
            request.UsuarioId,
            request.PageNumber,
            request.PageSize,
            request.CategoriaId,
            request.DataInicio,
            request.DataFim,
            request.TipoMovimentacao,
            request.Tag
        );
        var responseItems = items.Select(m => new MovimentacaoHistoricoResponse(
            m.Categoria,
            m.DataMovimentacao,
            m.Descricao,
            m.Valor,
            m.TipoMovimentacao,
            m.Tag
        ));
        return new PagedResponse<MovimentacaoHistoricoResponse>(
            responseItems,
            request.PageNumber,
            request.PageSize,
            totalCount
        );
    }
}
