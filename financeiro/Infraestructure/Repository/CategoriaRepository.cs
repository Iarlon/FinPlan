using Dapper;
using Financeiro.Domain.Enums;
using Financeiro.Domain.Repository;
using Financeiro.Infraestructure.Database;

namespace Financeiro.Infraestructure.Repository;

public class CategoriaRepository : ICategoriaRepository
{
        private readonly IUnitOfWork _uow;
    
        public CategoriaRepository(IUnitOfWork uow) => _uow = uow;
    
        public Task<CategoriaEnum?> ObterCategoriaPorId(int id)
        {
            var sql = @"
                SELECT id, nome, eh_receita AS EhReceita FROM categorias WHERE id = @Id
            ";
            return _uow.Connection.QueryFirstOrDefaultAsync<CategoriaEnum?>(sql, new { Id = id }, _uow.Transaction);
    }
}
