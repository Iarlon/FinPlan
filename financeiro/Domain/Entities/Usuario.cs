using Financeiro.Domain.Exceptions;

namespace Financeiro.Domain.Entities;

public class Usuario
{
    public long Id { get; private set; }
    public required string Nome { get; private set; }
    public required string Email { get; private set; }
    public required string Senha { get; private set; }

    public Usuario() { }

    public Usuario(string nome, string email, string senha)
    {
        AlterarNome(nome);
        AlterarEmail(email);
        AlterarSenha(senha);
    }

    public void AlterarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("O email não pode ser vazio.");
        Email = email;
    }

    public void AlterarSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new DomainException("Senha inválida.");
        Senha = senha;
    }

    public void AlterarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome não pode ser vazio.");
        Nome = nome;
    }
    public void DefinirId(long id)
    {
        if (id <= 0)
            throw new DomainException("Id inválido.");

        Id = id;
    }
}
