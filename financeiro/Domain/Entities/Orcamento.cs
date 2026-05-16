using Financeiro.Domain.Enums;
using Financeiro.Domain.Exceptions;

namespace Financeiro.Domain.Entities;

public class Orcamento
{
    public long Id { get; private set; }
    public int UsuarioId { get; private set; }
    public decimal SaldoConta { get; private set; } = decimal.Zero;

    public Orcamento(int usuarioId, DateTime periodo)
    {
        DefineUsuarioId(usuarioId);   
    }
    public void DefineUsuarioId(int usuarioId)
    {
        if (usuarioId <= 0)
            throw new DomainException("Não foi encontrado usuário para esse orçamento.");
        UsuarioId = usuarioId;
    }

    public void AtualizarSaldo(decimal valor, TipoMovimentacaoEnum tipo)
    {
        if (tipo == TipoMovimentacaoEnum.despesa)
            SaldoConta -= valor;
        else
            SaldoConta += valor;
    }
}
