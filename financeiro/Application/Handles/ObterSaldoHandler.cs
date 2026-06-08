using Financeiro.Application.Queries;
using Financeiro.Domain.Repository;
using MediatR;

namespace Financeiro.Application.Handles;

public class ObterSaldoHandler : IRequestHandler<ObterSaldoQuery, decimal>
{
    private readonly IOrcamentoRepository _orcamentoRepository;

    public ObterSaldoHandler(IOrcamentoRepository orcamentoRepository)
    {
        _orcamentoRepository = orcamentoRepository;
    }
    public async Task<decimal> Handle(ObterSaldoQuery request, CancellationToken cancellationToken)
    {
        var orcamento = await _orcamentoRepository.ObterSaldoPorUsuarioId(request.UsuarioId);
        return orcamento?.SaldoConta ?? 0;
    }
}
