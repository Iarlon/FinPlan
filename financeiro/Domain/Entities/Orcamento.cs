using Financeiro.Domain.Enums;
using Financeiro.Domain.Exceptions;

namespace Financeiro.Domain.Entities;

public class Orcamento
{
    public long Id { get; private set; }
    public long UsuarioId { get; private set; }
    public decimal SaldoConta { get; private set; } = decimal.Zero;

    private Orcamento()
    {
    }
    private Orcamento(long usuarioId)
    {
        if (usuarioId <= 0)
            throw new DomainException("Usuário inválido para orçamento.");

        UsuarioId = usuarioId;
        SaldoConta = 0;
    }

    public static Orcamento Criar(long usuarioId)
    {
        return new Orcamento(usuarioId);
    }

    public void DefinirId(long id)
    {
        if (id <= 0)
            throw new DomainException("Id inválido.");

        Id = id;
    }

    public void AtualizarSaldo(decimal valor, TipoMovimentacaoEnum tipo)
    {
        if (valor <= 0)
            throw new DomainException("Valor inválido para movimentação.");

        if (tipo == TipoMovimentacaoEnum.despesa)
            SaldoConta -= valor;
        else
            SaldoConta += valor;
    }
}
