using Dapper;
using Financeiro.Domain.Entities;
using Financeiro.Domain.Repository;
using Financeiro.Infraestructure.Database;

namespace Financeiro.Infraestructure.Repository;

public class MovimentacaoRepository : IMovimentacaoRepository
{
    private readonly IUnitOfWork _uow;

    public MovimentacaoRepository(IUnitOfWork uow) => _uow = uow;

    public Task AdicionarMovimentacao(Movimentacao movimentacao)
    {
        var sql = @"
            INSERT INTO MOVIMENTACAO (descricao, valor, data, tipo_id, categoria_id)
            VALUES (@Descricao, @Valor, @Data, @Tipo, @Categoria);
        ";
        return _uow.Connection.ExecuteAsync(sql, new
        {
            movimentacao.Descricao,
            movimentacao.Valor,
            movimentacao.DataMovimentacao,
            Tipo = (int)movimentacao.Tipo,
            Categoria = (int)movimentacao.Categoria
        }, _uow.Transaction);
    }

    public Task<Movimentacao> ObterMovimentacaoPorId(long id)
    {
        var sql = @"SELECT * FROM MOVIMENTACAO WHERE ID = @Id";
        return _uow.Connection.QueryFirstOrDefaultAsync<Movimentacao>(sql, new { Id = id }, _uow.Transaction);
    }

    public Task AtualizarMovimentacao(Movimentacao movimentacao)
    {
        if (movimentacao.Id <= 0)
            throw new ArgumentException("Id inválido.", nameof(movimentacao.Id));
        var sql = @"
            UPDATE MOVIMENTACAO
            SET descricao = @Descricao,
                valor = @Valor,
                data_movimentacao = @DataMovimentacao,
                tipo_id = @Tipo,
                categoria_id = @Categoria,
                tag = @Tag,
                usuario_id = @UsuarioId,
                orcamento_id = @OrcamentoId
                
            WHERE ID = @Id
        ";
        return _uow.Connection.ExecuteAsync(sql, new
        {
            movimentacao.Descricao,
            movimentacao.Valor,
            movimentacao.DataMovimentacao,
            Tipo = (int)movimentacao.Tipo,
            Categoria = (int)movimentacao.Categoria,
            movimentacao.Id,
            movimentacao.Tag,
            movimentacao.UsuarioId,
            movimentacao.OrcamentoId
        }, _uow.Transaction);
    }
}
