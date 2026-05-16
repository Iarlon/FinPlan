using Dapper;
using financeiro.Domain.Repository;
using financeiro.Infraestructure.Database;
using Financeiro.Domain.Entities;
using Financeiro.Infraestructure.Database;

namespace financeiro.Infraestructure.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IUnitOfWork _uow;

    public UsuarioRepository(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task CriarUsuario(Usuario usuario)
    {
        var sql = @"
            INSERT INTO USUARIO (nome, email, senha)
            VALUES (@Nome, @Email, @Senha)";
        await _uow.Connection.ExecuteAsync(sql, usuario);
    }

    public async Task<bool> EmailJaExiste(string email)
    {
        var sql = @"
        SELECT EXISTS (
            SELECT 1 FROM USUARIO WHERE Email = @Email COLLATE NOCASE
        )";
        var existe = await _uow.Connection.ExecuteScalarAsync<bool>(
        sql,
        new { Email = email }
    );

        return existe;
    }

    public async Task AtualizarUsuario(Usuario usuario)
    {
        var sql = @"
        UPDATE USUARIO
        SET NOME = @Nome,
            EMAIL = @Email
        WHERE id = @Id
        ";

       
        await _uow.Connection.ExecuteAsync(sql, usuario);
    }

    public async Task<Usuario?> ObterUsuarioPorId(int id)
    {
        var sql = @"SELECT * FROM USUARIO WHERE ID = @Id";

       
        var usuario = await _uow.Connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = id });

        return usuario;
    }
}
