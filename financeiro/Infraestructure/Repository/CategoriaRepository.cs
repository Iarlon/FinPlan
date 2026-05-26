using Dapper;
using Financeiro.Domain.Entities;
using Financeiro.Domain.Enums;
using Financeiro.Domain.Repository;
using Financeiro.Infraestructure.Database;
using Financeiro.Infraestructure.Model;

namespace Financeiro.Infraestructure.Repository;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly IUnitOfWork _uow;
    
    public CategoriaRepository(IUnitOfWork uow) => _uow = uow;
    
    public Task<Categoria> ObterCategoriaPorId(int id)
    {
        var sql = @"
            SELECT id, descricao, tipo_movimentacao_id::int AS tipo FROM categoria WHERE id = @Id
        ";
        return _uow.Connection.QueryFirstOrDefaultAsync<Categoria>(sql, new { Id = id }, _uow.Transaction);
    }
    public Task<IEnumerable<CategoriaReadModel>> ObterCategorias()
    {
        var sql = @"
            SELECT id, descricao, tipo_movimentacao_id::int AS tipo FROM categoria
        ";
        return _uow.Connection.QueryAsync<CategoriaReadModel>(sql, transaction: _uow.Transaction);
    }
}
