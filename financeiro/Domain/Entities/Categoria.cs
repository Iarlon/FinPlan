using Financeiro.Domain.Enums;

namespace Financeiro.Domain.Entities;

public class Categoria
{
    public int Id { get; private set; }
    public string Descricao { get; private set; }
    public bool EhReceita { get; private set; }

    public Categoria(int id, string descricao, bool ehReceita)
    {
        Id = id;
        Descricao = descricao;
        EhReceita = ehReceita;
    }

    public TipoMovimentacaoEnum Tipo => EhReceita
        ? TipoMovimentacaoEnum.receita
        : TipoMovimentacaoEnum.despesa;

    public bool EhCompativelCom(TipoMovimentacaoEnum tipoMovimentacao)
    {
        return this.Tipo == tipoMovimentacao;
    }
}
