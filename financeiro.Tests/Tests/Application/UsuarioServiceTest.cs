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
}
