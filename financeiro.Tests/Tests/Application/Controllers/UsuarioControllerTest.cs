namespace financeiro.Tests.Application.Controllers;

using financeiro.Application.Controllers;
using financeiro.Domain.Request;
using Financeiro.Application.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class UsuarioControllerTest
{
    private readonly Mock<IMediator>
        _mediatorMock;

    private readonly UsuarioController _controller;

    public UsuarioControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();

        _controller = new UsuarioController(
            _mediatorMock.Object);
    }

    [Fact]
    public async Task
        CriarUsuario_DadosValidos_DeveRetornarStatusCode201()
    {
        // Arrange

        var request = new CriarUsuarioRequest
        {
            Nome = "Iarlon",
            Email = "ialon@email.com",
            Senha = "123456"
        };

        _mediatorMock
            .Setup(x =>
                x.Send(
                    It.IsAny<CriarUsuarioCommand>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act

        var result = await _controller.CriarUsuario(request);

        // Assert

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, objectResult.StatusCode);
    }

    [Fact]
    public async Task
        CriarUsuario_DadosValidos_DeveEnviarCommandParoMediator()
    {
        // Arrange

        var request = new CriarUsuarioRequest
        {
            Nome = "Iarlon",
            Email = "ialon@email.com",
            Senha = "123456"
        };

        _mediatorMock
            .Setup(x =>
                x.Send(
                    It.IsAny<CriarUsuarioCommand>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act

        await _controller.CriarUsuario(request);

        // Assert

        _mediatorMock.Verify(
            x => x.Send(
                It.IsAny<CriarUsuarioCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task
        CriarUsuario_DadosValidos_DevePossuirMensagem()
    {
        // Arrange

        var request = new CriarUsuarioRequest
        {
            Nome = "João",
            Email = "joao@email.com",
            Senha = "senha123"
        };

        _mediatorMock
            .Setup(x =>
                x.Send(
                    It.IsAny<CriarUsuarioCommand>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);

        // Act

        var result = await _controller.CriarUsuario(request);

        // Assert

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.NotNull(objectResult.Value);
    }

    [Fact]
    public async Task
        CriarUsuario_DadosValidos_DevePassarParametrosCorretos()
    {
        // Arrange

        var request = new CriarUsuarioRequest
        {
            Nome = "Maria",
            Email = "maria@email.com",
            Senha = "senha456"
        };

        _mediatorMock
            .Setup(x =>
                x.Send(
                    It.IsAny<CriarUsuarioCommand>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(3)
            .Callback(new InvocationAction(invocation =>
            {
                var command = invocation.Arguments[0] as CriarUsuarioCommand;
                Assert.NotNull(command);
                Assert.Equal(request.Nome, command.Nome);
                Assert.Equal(request.Email, command.Email);
                Assert.Equal(request.Senha, command.Senha);
            }));

        // Act

        await _controller.CriarUsuario(request);

        // Assert

        _mediatorMock.Verify(
            x => x.Send(
                It.IsAny<CriarUsuarioCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
