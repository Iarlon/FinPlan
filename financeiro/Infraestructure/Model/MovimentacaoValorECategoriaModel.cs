using Financeiro.Domain.Enums;

namespace Financeiro.Infraestructure.Model;

public record MovimentacaoValorECategoriaModel(string Categoria, string DescricaoMovimentacao, decimal Valor, TipoMovimentacaoEnum Tipo);
