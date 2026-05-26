using Dapper;
using financeiro.Domain.Repository;
using financeiro.Infraestructure.Database;
using Financeiro.Domain.Entities;
using Financeiro.Infraestructure.Database;
using Financeiro.Infraestructure.Model;

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
        INSERT INTO usuario (nome, email, senha)
        VALUES (@Nome, @Email, @Senha)
        RETURNING id;
        ";

        var id = await _uow.Connection.ExecuteScalarAsync<long>(
            sql,
            usuario,
            _uow.Transaction);

        usuario.DefinirId(id);
    }
    public async Task<Usuario> ObterUsuarioPorEmail(string email)
    {
        var sql = @"
            SELECT U.ID, U.NOME, U.EMAIL, U.SENHA
            FROM USUARIO U WHERE U.EMAIL = @Email";
        var usuario = await _uow.Connection
            .QueryFirstOrDefaultAsync<Usuario>(
            sql,
            new { Email = email },
            _uow.Transaction);

        return usuario;
    }

    public async Task<bool> EmailJaExiste(string email)
    {
        var sql = @"
        SELECT EXISTS (
            SELECT 1
            FROM usuario
            WHERE email ILIKE @Email
        )";
        var existe = await _uow.Connection.ExecuteScalarAsync<bool>(
        sql,
        new { Email = email },
        _uow.Transaction
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

       
        await _uow.Connection.ExecuteAsync(sql, usuario, _uow.Transaction);
    }

    public async Task<Usuario> ObterUsuarioPorId(long id)
    {
        var sql = @"SELECT * FROM USUARIO WHERE ID = @Id";

       
        var usuario = await _uow.Connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = id }, _uow.Transaction);

        return usuario;
    }
}
