using financeiro.Application.Contract;
using financeiro.Domain.Events;
using Financeiro.Domain.Repository;
using Financeiro.Infraestructure.Database;
using MediatR;

namespace Financeiro.Application.Handles;

public class AtualizarSaldoOrcamentoHandler
    : INotificationHandler<MovimentacaoCriadaEvent>
{
    private readonly IOrcamentoRepository _orcamentoRepository;
    private readonly IUnitOfWork _uow;

    public AtualizarSaldoOrcamentoHandler(IOrcamentoRepository orcamentoRepository, IUnitOfWork uow)
    {
        _uow = uow;
        _orcamentoRepository = orcamentoRepository;
    }

    public async Task Handle(
        MovimentacaoCriadaEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var orcamento = await _orcamentoRepository.ObterOrcamentoPorId(domainEvent.Movimentacao.OrcamentoId);

        orcamento.AtualizarSaldo(domainEvent.Movimentacao.Valor, domainEvent.Movimentacao.Tipo);

        await _orcamentoRepository.AtualizarOrcamento(orcamento);
    }
}
