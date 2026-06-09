using Dapper;
using Financeiro.Application.Response;
using Financeiro.Domain.Entities;
using Financeiro.Domain.Repository;
using Financeiro.Infraestructure.Database;
using Financeiro.Infraestructure.Model;
using MediatR;
using System.Text;

namespace Financeiro.Infraestructure.Repository;

public class MovimentacaoRepository : IMovimentacaoRepository
{
    private readonly IUnitOfWork _uow;

    public MovimentacaoRepository(IUnitOfWork uow) => _uow = uow;

    public async Task AdicionarMovimentacao(Movimentacao movimentacao)
    {
        var sql = @"
        INSERT INTO movimentacao (descricao, valor, data_movimentacao, categoria_id, usuario_id, orcamento_id, tag)
        VALUES (@Descricao, @Valor, @DataMovimentacao, @CategoriaId, @UsuarioId, @OrcamentoId, @Tag)
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
                CategoriaId = movimentacao.Categoria.Id,
                movimentacao.Tag
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
            SELECT
                MOV.VALOR as Valor,
                CAT.DESCRICAO AS Categoria,
                CAT.TIPO_MOVIMENTACAO_ID AS TipoMovimentacao
            FROM MOVIMENTACAO MOV
            JOIN CATEGORIA CAT ON CAT.ID = MOV.CATEGORIA_ID
            WHERE USUARIO_ID = @usuarioId
        ";
        return _uow.Connection.QueryAsync<MovimentacaoValorECategoriaModel>(sql, new {usuarioId}, _uow.Transaction);
    }

    public Task<IEnumerable<MovimentacaoValorDataModel>> ObterMovimentacaoPorPeriodo(long usuarioId, DateTime inicio, DateTime fim)
    {
        var sql = @"
            SELECT
                M.VALOR, 
                M.DATA_MOVIMENTACAO, 
                C.TIPO_MOVIMENTACAO_ID as tipo_movimentacao
            FROM MOVIMENTACAO M
            JOIN CATEGORIA C ON C.ID = M.CATEGORIA_ID
            WHERE M.USUARIO_ID = @UsuarioId
                AND M.DATA_MOVIMENTACAO >= @Inicio
                AND M.DATA_MOVIMENTACAO <= @Fim";
        return _uow.Connection.QueryAsync<MovimentacaoValorDataModel>(sql, new { UsuarioId = usuarioId, Inicio = inicio, Fim = fim }, _uow.Transaction);
    }

    public Task<IEnumerable<MovimentacaoRecenteModel>> ObterMovimentacaoRecente(long usuarioId)
    {
        if (usuarioId <= 0)
            throw new ArgumentException("Usuario não encontrado.", nameof(usuarioId));
        var sql = @"
            SELECT
                MOV.DESCRICAO AS DescricaoMovimentacao,
                MOV.VALOR as Valor,
                MOV.DATA_MOVIMENTACAO as DataMovimentacao,
                MOV.TAG as Tag,
                CAT.DESCRICAO AS Categoria,
                CAT.TIPO_MOVIMENTACAO_ID AS TipoMovimentacao
            FROM MOVIMENTACAO MOV
            JOIN CATEGORIA CAT ON CAT.ID = MOV.CATEGORIA_ID
            WHERE USUARIO_ID = @usuarioId
        ";
        return _uow.Connection.QueryAsync<MovimentacaoRecenteModel>(sql, new { usuarioId }, _uow.Transaction);
    }

    public async Task<(IEnumerable<MovimentacaoHistoricoModel> Items, int TotalCount)>
        ObterHistoricoPaginado(long usuarioId,
            int pageNumber,
            int pageSize,
            long? categoriaId,
            DateTime dataInicio,
            DateTime dataFim,
            int? tipoMovimentacao,
            string? tag)
    {
        var sqlTemplate = @"
        SELECT M.DATA_MOVIMENTACAO as DataMovimentacao, M.VALOR, C.DESCRICAO as Categoria, M.DESCRICAO, M.TAG, C.TIPO_MOVIMENTACAO_ID AS TipoMovimentacao
        FROM MOVIMENTACAO M
        INNER JOIN CATEGORIA C ON C.ID = M.CATEGORIA_ID
        /**where**/
        ORDER BY M.DATA_MOVIMENTACAO DESC, M.ID DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        var sqlWhere = new StringBuilder("WHERE M.USUARIO_ID = @UsuarioId");
        var parameters = new DynamicParameters();

        parameters.Add("UsuarioId", usuarioId);
        parameters.Add("Offset", (pageNumber - 1) * pageSize);
        parameters.Add("PageSize", pageSize);

        if (categoriaId.HasValue)
        {
            sqlWhere.Append(" AND M.CATEGORIA_ID = @CategoriaId");
            parameters.Add("CategoriaId", categoriaId.Value);
        }

        if (dataInicio != default)
        {
            sqlWhere.Append(" AND M.DATA_MOVIMENTACAO >= @DataInicio");
            parameters.Add("DataInicio", dataInicio);
        }

        if (dataFim != default)
        {
            sqlWhere.Append(" AND M.DATA_MOVIMENTACAO <= @DataFim");
            parameters.Add("DataFim", dataFim);
        }

        if (tipoMovimentacao.HasValue)
        {
            sqlWhere.Append(" AND C.TIPO_MOVIMENTACAO_ID = @TipoMovimentacao");
            parameters.Add("TipoMovimentacao", tipoMovimentacao.Value);
        }

        if (tag != null)
        {
            sqlWhere.Append(" AND M.TAG = @Tag");
            parameters.Add("Tag", tag);
        }

        var countSql = $"SELECT COUNT(*) FROM MOVIMENTACAO M INNER JOIN CATEGORIA C ON C.ID = M.CATEGORIA_ID {sqlWhere}";
        var totalCount = await _uow.Connection.ExecuteScalarAsync<int>(countSql, parameters, _uow.Transaction);

        var sqlFinal = sqlTemplate.Replace("/**where**/", sqlWhere.ToString());
        var items = await _uow.Connection.QueryAsync<MovimentacaoHistoricoModel>(sqlFinal, parameters, _uow.Transaction);

        return (items, totalCount);
    }
}
