using financeiro.Application.Contract;
using financeiro.Domain.Repository;
using Financeiro.Application.Command;
using Financeiro.Application.Contract;
using Financeiro.Application.Response;
using Financeiro.Domain.Entities;
using Financeiro.Domain.Exceptions;
using MediatR;

namespace Financeiro.Application.Handles;

public class LoginHandler
    : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHashService _hashService;
    private readonly ITokenService _tokenService;

    public LoginHandler(
        IUsuarioRepository usuarioRepository,
        IHashService hashService,
        ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _hashService = hashService;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var usuarioModel =
            await _usuarioRepository.ObterUsuarioPorEmail(
                request.Email);

        if (usuarioModel is null ||
            !_hashService.VerificarHash(
                request.Senha,
                usuarioModel.SenhaHash))
        {
            throw new DomainException(
                "Usuário ou senha inválidos.");
        }

        var usuario = new Usuario(
            usuarioModel.Nome,
            usuarioModel.Email,
            usuarioModel.SenhaHash
            );

        var token = _tokenService.GerarToken(usuario);

        return new LoginResponse(token);
    }
}
