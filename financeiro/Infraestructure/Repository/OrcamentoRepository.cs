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

    public async Task<long> ObterOuCriarOrcamentoId(long usuarioId) {
        if (usuarioId <= 0)
            throw new ArgumentException("Id inválido.", nameof(usuarioId));

        var sql = @"
            INSERT INTO orcamento (usuario_id, saldo_conta)
            VALUES (@usuarioId, 0)
            ON CONFLICT (usuario_id)
            DO UPDATE SET usuario_id = EXCLUDED.usuario_id
            RETURNING id;
        ";

        return await _uow.Connection.ExecuteScalarAsync<long>(
            sql,
            new { usuarioId },
            _uow.Transaction
        );
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
            Valor = orcamento.SaldoConta,
        }, _uow.Transaction);
    }

    public async Task<Orcamento> ObterOrcamentoPorId(long id)
    {
        var sql = @"SELECT * FROM ORCAMENTO WHERE ID = @Id";

        var orcamento = await _uow.Connection.QueryFirstOrDefaultAsync<Orcamento>(sql, new
        { Id = id },
        _uow.Transaction);

        return orcamento;
    }

    public async Task<Orcamento> ObterSaldoPorUsuarioId(long usuarioId)
    {
        var sql = @"SELECT * FROM ORCAMENTO WHERE USUARIO_ID = @UsuarioId";
        var orcamento = await _uow.Connection.QueryFirstOrDefaultAsync<Orcamento>(sql, new
        { UsuarioId = usuarioId },
        _uow.Transaction);
        return orcamento;
    }
}
