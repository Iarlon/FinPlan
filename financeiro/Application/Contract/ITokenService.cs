using Financeiro.Domain.Entities;

namespace Financeiro.Application.Contract;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}
