using Financeiro.Application.Queries;
using Financeiro.Application.Response;
using Financeiro.Domain.Repository;
using MediatR;

namespace Financeiro.Application.Handles;

public class ObterMovimentacaoPorIdHandler
    : IRequestHandler<ObterMovimentacaoPorIdQuery, MovimentacaoResponse?>
{
    private readonly IMovimentacaoRepository _repository;

    public ObterMovimentacaoPorIdHandler(IMovimentacaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<MovimentacaoResponse?> Handle(
        ObterMovimentacaoPorIdQuery request,
        CancellationToken cancellationToken)
    {
        var mov = await _repository.ObterMovimentacaoPorId(request.Id);

        if (mov is null)
            return null;

        return new MovimentacaoResponse(
            mov.Id,
            mov.Valor,
            mov.Tipo.ToString(),
            mov.Descricao,
            mov.Categoria.ToString(),
            mov.DataMovimentacao);
    }
}
