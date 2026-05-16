using Dapper;
using financeiro.Domain.Repository;
using financeiro.Infraestructure.Database;
using Financeiro.Domain.Entities;
using Financeiro.Domain.Repository;
using Financeiro.Infraestructure.Database;

namespace Financeiro.Infraestructure.Repository;

public class OrcamentoRepository : IOrcamentoRepository
{
    private readonly IUnitOfWork _uow;

    public OrcamentoRepository(IUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task AtualizarOrcamento(Orcamento orcamento)
    {
        if (orcamento.Id <= 0)
            throw new ArgumentException("Id inválido.", nameof(orcamento.Id));

        var sql = @"
            UPDATE ORCAMENTO
            SET saldo_conta = @Valor
            WHERE ID = @Id
            ";
        await _uow.Connection.ExecuteAsync(sql, new {
            orcamento.Id,
            Valor = orcamento.SaldoConta
        });
    }

    public async Task<Orcamento?> ObterOrcamentoPorId(int id)
    {
        var sql = @"SELECT * FROM ORCAMENTO WHERE ID = @Id";

        var orcamento = await _uow.Connection.QueryFirstOrDefaultAsync<Orcamento>(sql, new { Id = id });

        return orcamento;
    }
}
