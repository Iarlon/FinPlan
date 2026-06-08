using Financeiro.Application.Queries;
using Financeiro.Application.Response;
using Financeiro.Domain.Enums;
using Financeiro.Domain.Repository;
using MediatR;

namespace Financeiro.Application.Handles;

public class ObterResumoSaldoHandler :
    IRequestHandler<ObterResumoSaldoQuery, ResumoSaldoResponse>
{
    private readonly IOrcamentoRepository _orcamentoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IMovimentacaoRepository _movimentacaoRepository;

    public ObterResumoSaldoHandler(
        ICategoriaRepository categoriaRepository,
        IOrcamentoRepository orcamentoRepository,
        IMovimentacaoRepository movimentacaoRepository)
    {
        _categoriaRepository = categoriaRepository;
        _orcamentoRepository = orcamentoRepository;
        _movimentacaoRepository = movimentacaoRepository;
    }

    public async Task<ResumoSaldoResponse> Handle(ObterResumoSaldoQuery request, CancellationToken cancellationToken)
    {
        var orcamento = await _orcamentoRepository.ObterSaldoPorUsuarioId(request.UsuarioId);
        decimal saldoAtualTotal = orcamento?.SaldoConta ?? 0;

        var hoje = DateTime.UtcNow;
        var inicioMesAtual = new DateTime(hoje.Year, hoje.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var inicioMesAnterior = inicioMesAtual.AddMonths(-1);

        var movimentacoesPeriodo = await _movimentacaoRepository.ObterMovimentacaoPorPeriodo(
            request.UsuarioId,
            inicioMesAnterior,
            hoje);

        if (movimentacoesPeriodo == null || !movimentacoesPeriodo.Any())
        {
            return new ResumoSaldoResponse(0, true);
        }

        decimal liquidoMesAtual = movimentacoesPeriodo
            .Where(m => m.DataMovimentacao >= inicioMesAtual)
            .Sum(m => m.ValorComSinal);

        decimal liquidoMesAnterior = movimentacoesPeriodo
            .Where(m => m.DataMovimentacao >= inicioMesAnterior && m.DataMovimentacao < inicioMesAtual)
            .Sum(m => m.ValorComSinal);

        var percentualVariacao = liquidoMesAnterior != 0
            ? ((liquidoMesAtual - liquidoMesAnterior) / Math.Abs(liquidoMesAnterior)) * 100
            : liquidoMesAtual > 0 ? 100 : 0;

        bool tendenciaPositiva = liquidoMesAtual >= liquidoMesAnterior;

        return new ResumoSaldoResponse(
            Math.Round(percentualVariacao, 2),
            tendenciaPositiva
        );
    }
}
