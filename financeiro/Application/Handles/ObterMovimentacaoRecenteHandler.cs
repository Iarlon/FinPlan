using Financeiro.Application.Queries;
using Financeiro.Application.Response;
using Financeiro.Domain.Repository;
using MediatR;

namespace Financeiro.Application.Handles;

public class ObterMovimentacaoRecenteHandler : IRequestHandler<ObterMovimentacaoRecenteQuery, IEnumerable<MovimentacaoRecentesResponse>>
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;

    public ObterMovimentacaoRecenteHandler(IMovimentacaoRepository movimentacaoRepository)
    {
        _movimentacaoRepository = movimentacaoRepository;
    }

    public async Task<IEnumerable<MovimentacaoRecentesResponse>> Handle(ObterMovimentacaoRecenteQuery request, CancellationToken cancellationToken)
    {
        var movimentacoes = await _movimentacaoRepository.ObterMovimentacaoRecente(request.UsuarioId);
        return movimentacoes.Select(m => new MovimentacaoRecentesResponse(
            m.Categoria,
            m.DataMovimentacao,
            m.DescricaoMovimentacao,
            m.Valor,
            m.Tag));
    }
}
