namespace financeiro.Tests.Application;

using financeiro.Application.Contract;
using financeiro.Domain.Repository;
using Financeiro.Application.Command;
using Financeiro.Application.Contract;
using Financeiro.Application.Handles;
using Financeiro.Domain.Entities;
using Financeiro.Domain.Exceptions;
using Moq;
using Xunit;

public class LoginHandlerTest
{
    private readonly Mock<IUsuarioRepository>
        _usuarioRepositoryMock;

    private readonly Mock<IHashService>
        _hashServiceMock;

    private readonly Mock<ITokenService>
        _tokenServiceMock;

    private readonly LoginHandler _handler;

    public LoginHandlerTest()
    {
        _usuarioRepositoryMock =
            new Mock<IUsuarioRepository>();

        _hashServiceMock =
            new Mock<IHashService>();

        _tokenServiceMock =
            new Mock<ITokenService>();

        _handler = new LoginHandler(
            _usuarioRepositoryMock.Object,
            _hashServiceMock.Object,
            _tokenServiceMock.Object);
    }

    [Fact]
    public async Task
        Login_UsuarioNaoExiste_DeveLancarException()
    {
        // Arrange

        var command = new LoginCommand(
            "naoexiste@email.com",
            "123456");

        _usuarioRepositoryMock
            .Setup(x =>
                x.ObterUsuarioPorEmail(command.Email))
            .ReturnsAsync((Usuario)null!);

        // Act

        var exception =
            await Assert.ThrowsAsync<DomainException>(
                () => _handler.Handle(
                    command,
                    CancellationToken.None));

        // Assert

        Assert.Equal(
            "Usuário ou senha inválidos.",
            exception.Message);
    }

    [Fact]
    public async Task
        Login_SenhaInvalida_DeveLancarException()
    {
        // Arrange

        var usuario = new Usuario(
            "Iarlon",
            "ialon@email.com",
            "senhaHashada");

        var command = new LoginCommand(
            "ialon@email.com",
            "senhaErrada");

        _usuarioRepositoryMock
            .Setup(x =>
                x.ObterUsuarioPorEmail(command.Email))
            .ReturnsAsync(usuario);

        _hashServiceMock
            .Setup(x =>
                x.VerificarHash(command.Senha, usuario.Senha))
            .Returns(false);

        // Act

        var exception =
            await Assert.ThrowsAsync<DomainException>(
                () => _handler.Handle(
                    command,
                    CancellationToken.None));

        // Assert

        Assert.Equal(
            "Usuário ou senha inválidos.",
            exception.Message);
    }

    [Fact]
    public async Task
        Login_CredenciaisValidas_DeveRetornarToken()
    {
        // Arrange

        var usuario = new Usuario(
            "Iarlon",
            "ialon@email.com",
            "senhaHashada");

        var command = new LoginCommand(
            "ialon@email.com",
            "123456");

        const string tokenEsperado = "token_jwt_123";

        _usuarioRepositoryMock
            .Setup(x =>
                x.ObterUsuarioPorEmail(command.Email))
            .ReturnsAsync(usuario);

        _hashServiceMock
            .Setup(x =>
                x.VerificarHash(command.Senha, usuario.Senha))
            .Returns(true);

        _tokenServiceMock
            .Setup(x =>
                x.GerarToken(It.IsAny<Usuario>()))
            .Returns((string?)tokenEsperado);

        // Act

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        Assert.NotNull(result);
        Assert.Equal(tokenEsperado, result.Token);
    }

    [Fact]
    public async Task
        Login_CredenciaisValidas_DeveGerarToken()
    {
        // Arrange

        var usuario = new Usuario(
            "Iarlon",
            "ialon@email.com",
            "senhaHashada");

        var command = new LoginCommand(
            "ialon@email.com",
            "123456");

        _usuarioRepositoryMock
            .Setup(x =>
                x.ObterUsuarioPorEmail(command.Email))
            .ReturnsAsync(usuario);

        _hashServiceMock
            .Setup(x =>
                x.VerificarHash(command.Senha, usuario.Senha))
            .Returns(true);

        _tokenServiceMock
            .Setup(x =>
                x.GerarToken(It.IsAny<Usuario>()))
            .Returns((string?)"token");

        // Act

        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        _tokenServiceMock.Verify(
            x => x.GerarToken(It.IsAny<Usuario>()),
            Times.Once);
    }
}
