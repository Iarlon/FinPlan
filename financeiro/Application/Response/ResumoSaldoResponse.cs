namespace Financeiro.Application.Response;

public record ResumoSaldoResponse(
    decimal PercentualVariacao,
    bool TendenciaPositiva
);
