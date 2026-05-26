using Financeiro.Domain.Entities;
using Financeiro.Infraestructure.Model;

namespace financeiro.Domain.Repository;

public interface IUsuarioRepository
{
    Task<bool> EmailJaExiste(string email);
    Task CriarUsuario(Usuario usuario);

    Task AtualizarUsuario(Usuario usuario);
    Task<Usuario> ObterUsuarioPorId(long id);
    Task<Usuario> ObterUsuarioPorEmail(string email);
}
