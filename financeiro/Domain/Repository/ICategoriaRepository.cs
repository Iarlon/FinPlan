using Financeiro.Domain.Entities;
using Financeiro.Domain.Enums;
using Financeiro.Domain.Model;

namespace Financeiro.Domain.Repository;

public interface ICategoriaRepository
{
    Task<Categoria> ObterCategoriaPorId(int categoriaId);
    Task<IEnumerable<CategoriaReadModel>> ObterCategorias();
}
