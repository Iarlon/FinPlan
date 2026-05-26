using Financeiro.Domain.Enums;

namespace Financeiro.Infraestructure.Model;

public record MovimentacaoValorDataModel(decimal Valor, DateTime Data, TipoMovimentacaoEnum Tipo);
