using Financeiro.Application.Contract;
using Financeiro.Domain.Entities;
using Financeiro.Infraestructure.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Financeiro.Application.Service;

public class TokenService : ITokenService
{
    public readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration) => _configuration = configuration;
    public string GerarToken(Usuario usuario)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

        var handler = new JwtSecurityTokenHandler();

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
                ]),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = credentials
        };
        var token = handler.CreateToken(tokenDescriptor);
        var stringToken = handler.WriteToken(token);

        return stringToken;
    }
}
