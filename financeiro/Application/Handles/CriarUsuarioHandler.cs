using financeiro.Application.Contract;
using financeiro.Domain.Repository;
using Financeiro.Application.Command;
using Financeiro.Domain.Entities;
using Financeiro.Domain.Exceptions;
using Financeiro.Infraestructure.Database;
using MediatR;

namespace Financeiro.Application.Handles;

public class CriarUsuarioHandler : IRequestHandler<CriarUsuarioCommand, long>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHashService _hashService;
    private readonly IUnitOfWork _uow;
    public CriarUsuarioHandler(
        IUsuarioRepository usuarioRepository,
        IHashService hashService,
        IUnitOfWork uow)
    {
        _usuarioRepository = usuarioRepository;
        _hashService = hashService;
        _uow = uow;
    }
    public async Task<long> Handle(CriarUsuarioCommand request, CancellationToken ct)
    {
        bool emailJaExiste =
        await _usuarioRepository.EmailJaExiste(request.Email);

        if (emailJaExiste)
        {
            throw new DomainException("E-mail já cadastrado.");
        }

        var senhaHash = _hashService.GerarHash(request.Senha);
        var usuario = new Usuario(
            request.Nome,
            request.Email,
            senhaHash);

        await _usuarioRepository.CriarUsuario(usuario);
        await _uow.CommitAsync();
        return usuario.Id;
    }
}
