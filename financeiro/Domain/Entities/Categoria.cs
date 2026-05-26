using Financeiro.Domain.Enums;
using Financeiro.Domain.Exceptions;

namespace Financeiro.Domain.Entities;

public class Categoria
{
    public long Id { get; private set; }
    public string Descricao { get; private set; }
    public TipoMovimentacaoEnum Tipo { get; private set; }

    public Categoria()
    {

    }
    public Categoria(string descricao, TipoMovimentacaoEnum tipo)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição da categoria é obrigatória.");

        Descricao = descricao.Trim();
        Tipo = tipo;
    }
}
