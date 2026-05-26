using Dapper;
using Financeiro.Domain.Entities;
using Financeiro.Domain.Repository;
using Financeiro.Infraestructure.Database;
using Financeiro.Infraestructure.Model;

namespace Financeiro.Infraestructure.Repository;

public class MovimentacaoRepository : IMovimentacaoRepository
{
    private readonly IUnitOfWork _uow;

    public MovimentacaoRepository(IUnitOfWork uow) => _uow = uow;

    public async Task AdicionarMovimentacao(Movimentacao movimentacao)
    {
        var sql = @"
        INSERT INTO movimentacao (descricao, valor, data_movimentacao, categoria_id, usuario_id, orcamento_id)
        VALUES (@Descricao, @Valor, @DataMovimentacao, @CategoriaId, @UsuarioId, @OrcamentoId)
        RETURNING id;
    ";

        _uow.TrackEntity(movimentacao);

        var id = await _uow.Connection.ExecuteScalarAsync<long>(
            sql,
            new
            {
                movimentacao.Descricao,
                movimentacao.UsuarioId,
                movimentacao.OrcamentoId,
                movimentacao.Valor,
                movimentacao.DataMovimentacao,
                CategoriaId = movimentacao.Categoria.Id
            },
            _uow.Transaction);

        movimentacao.DefinirId(id);
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
                categoria_id = @CategoriaId,
                tag = @Tag,
                usuario_id = @UsuarioId,
                orcamento_id = @OrcamentoId
                
            WHERE ID = @Id
        ";
        _uow.TrackEntity(movimentacao);
        return _uow.Connection.ExecuteAsync(sql, new
        {
            movimentacao.Descricao,
            movimentacao.Valor,
            movimentacao.DataMovimentacao,
            CategoriaId = movimentacao.Categoria.Id,
            movimentacao.Id,
            movimentacao.Tag,
            movimentacao.UsuarioId,
            movimentacao.OrcamentoId
        }, _uow.Transaction);
    }

    public Task<IEnumerable<MovimentacaoValorECategoriaModel>> ObterValorECategoria(long usuarioId)
    {
        if (usuarioId <= 0)
            throw new ArgumentException("Usuario não encontrado.", nameof(usuarioId));
        var sql = @"
            SELECT MOV.DESCRICAO AS DescricaoMovimentacao, MOV.VALOR as Valor, CAT.DESCRICAO AS Categoria, TPM.TIPO AS Tipo
            FROM MOVIMENTACAO MOV
            JOIN CATEGORIA CAT ON CAT.ID = MOV.CATEGORIA_ID
            JOIN TIPO_MOVIMENTACAO TPM ON TPM.ID = MOV.TIPO_ID
            WHERE USUARIO_ID = @usuarioId
        ";
        return _uow.Connection.QueryFirstOrDefaultAsync<IEnumerable<MovimentacaoValorECategoriaModel>>(sql, new {usuarioId}, _uow.Transaction);
    }

    public Task<IEnumerable<MovimentacaoValorDataModel>> ObterMovimentacaoPorPeriodo(long usuarioId, DateTime inicio, DateTime fim)
    {
        var sql = @"
            SELECT VALOR, DATA_MOVIMENTACAO, TIPO
            FROM MOVIMENTACAO
            WHERE USUARIO_ID = @UsuarioId
            AND DATA_MOVIMENTACAO >= @Inicio
            AND DATA_MOVIMENTACAO <= @Fim";
        return _uow.Connection.QueryAsync<MovimentacaoValorDataModel>(sql, new { UsuarioId = usuarioId, Inicio = inicio, Fim = fim }, _uow.Transaction);
    }
}
