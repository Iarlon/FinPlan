namespace financeiro.Tests.Application;

using financeiro.Application.Contract;
using financeiro.Domain.Repository;
using Financeiro.Application.Command;
using Financeiro.Application.Handles;
using Financeiro.Domain.Exceptions;
using Financeiro.Infraestructure.Database;
using Moq;
using Xunit;

public class UsuarioServiceTest
{
    private readonly Mock<IUsuarioRepository>
        _usuarioRepositoryMock;

    private readonly Mock<IHashService>
        _hashServiceMock;

    private readonly Mock<IUnitOfWork>
        _uowMock;

    private readonly CriarUsuarioHandler _handler;

    public UsuarioServiceTest()
    {
        _uowMock = new Mock<IUnitOfWork>();

        _usuarioRepositoryMock =
            new Mock<IUsuarioRepository>();

        _hashServiceMock =
            new Mock<IHashService>();

        _handler = new CriarUsuarioHandler(
            _usuarioRepositoryMock.Object,
            _hashServiceMock.Object,
            _uowMock.Object);
    }

    [Fact]
    public async Task
        CriarUsuario_EmailJaExiste_DeveLancarException()
    {
        // Arrange

        var command = new CriarUsuarioCommand(
            "Iarlon",
            "ialon@email.com",
            "123456");

        _usuarioRepositoryMock
            .Setup(x =>
                x.EmailJaExiste(command.Email))
            .ReturnsAsync(true);

        // Act

        var exception =
            await Assert.ThrowsAsync<DomainException>(
                () => _handler.Handle(
                    command,
                    CancellationToken.None));

        // Assert

        Assert.Equal(
            "E-mail já cadastrado.",
            exception.Message);
    }

    [Fact]
    public async Task
        CriarUsuario_ValorValido_DeveRetornarIdDoUsuario()
    {
        // Arrange

        var command = new CriarUsuarioCommand(
            "Iarlon",
            "ialon@email.com",
            "123456");

        _usuarioRepositoryMock
            .Setup(x =>
                x.EmailJaExiste(command.Email))
            .ReturnsAsync(false);

        _hashServiceMock
            .Setup(x =>
                x.GerarHash(command.Senha))
            .Returns("senhaHashada");

        _uowMock
            .Setup(x =>
                x.CommitAsync())
            .Returns(Task.CompletedTask);

        // Act

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        Assert.Equal(0, result);
        _usuarioRepositoryMock.Verify(
            x => x.CriarUsuario(It.IsAny<Financeiro.Domain.Entities.Usuario>()),
            Times.Once);
        _uowMock.Verify(
            x => x.CommitAsync(),
            Times.Once);
    }

    [Fact]
    public async Task
        CriarUsuario_UsuarioValido_DeveGerarHashDaSenha()
    {
        // Arrange

        var command = new CriarUsuarioCommand(
            "Iarlon",
            "ialon@email.com",
            "123456");

        _usuarioRepositoryMock
            .Setup(x =>
                x.EmailJaExiste(command.Email))
            .ReturnsAsync(false);

        _hashServiceMock
            .Setup(x =>
                x.GerarHash(command.Senha))
            .Returns("senhaHashada");

        _uowMock
            .Setup(x =>
                x.CommitAsync())
            .Returns(Task.CompletedTask);

        // Act

        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        _hashServiceMock.Verify(
            x => x.GerarHash(command.Senha),
            Times.Once);
    }

    [Fact]
    public async Task
        CriarUsuario_UsuarioValido_DeveCommitarTransacao()
    {
        // Arrange

        var command = new CriarUsuarioCommand(
            "João",
            "joao@email.com",
            "senha123");

        _usuarioRepositoryMock
            .Setup(x =>
                x.EmailJaExiste(command.Email))
            .ReturnsAsync(false);

        _hashServiceMock
            .Setup(x =>
                x.GerarHash(It.IsAny<string>()))
            .Returns("hash");

        _uowMock
            .Setup(x =>
                x.CommitAsync())
            .Returns(Task.CompletedTask);

        // Act

        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        _uowMock.Verify(
            x => x.CommitAsync(),
            Times.Once);
    }
}
