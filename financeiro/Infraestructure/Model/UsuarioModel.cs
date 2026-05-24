namespace Financeiro.Infraestructure.Model;

public class UsuarioModel
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string SenhaHash { get; set; }
}
