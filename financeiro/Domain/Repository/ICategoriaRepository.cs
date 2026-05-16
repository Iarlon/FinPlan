using Financeiro.Domain.Enums;

namespace Financeiro.Domain.Repository;

public interface ICategoriaRepository
{
    Task<CategoriaEnum?> ObterCategoriaPorId(int categoriaId);
}
