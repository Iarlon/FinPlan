namespace financeiro.Tests.Application.Controllers;

using Financeiro.Application.Command;
using Financeiro.Application.Controllers;
using Financeiro.Application.Request;
using Financeiro.Application.Response;
using Financeiro.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class AuthControllerTest
{
    private readonly Mock<IMediator>
        _mediatorMock;

    private readonly AuthController _controller;

    public AuthControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();

        _controller = new AuthController(
            _mediatorMock.Object);
    }

    [Fact]
    public async Task
        Login_CredenciaisValidas_DeveRetornarOk()
    {
        // Arrange

        var request = new LoginRequest(
            "ialon@email.com",
            "123456");

        var response = new LoginResponse("token_123");

        _mediatorMock
            .Setup(x =>
                x.Send(
                    It.IsAny<LoginCommand>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act

        var result = await _controller.Login(request);

        // Assert

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task
        Login_CredenciaisValidas_DeveRetornarToken()
    {
        // Arrange

        var request = new LoginRequest(
            "ialon@email.com",
            "123456");

        const string tokenEsperado = "token_jwt_123";
        var response = new LoginResponse(tokenEsperado);

        _mediatorMock
            .Setup(x =>
                x.Send(
                    It.IsAny<LoginCommand>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act

        var result = await _controller.Login(request);

        // Assert

        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseResult = Assert.IsType<LoginResponse>(okResult.Value);
        Assert.Equal(tokenEsperado, responseResult.Token);
    }

    [Fact]
    public async Task
        Login_CredenciaisValidas_DeveEnviarCommandAoMediator()
    {
        // Arrange

        var request = new LoginRequest(
            "ialon@email.com",
            "123456");

        var response = new LoginResponse("token");

        _mediatorMock
            .Setup(x =>
                x.Send(
                    It.IsAny<LoginCommand>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act

        await _controller.Login(request);

        // Assert

        _mediatorMock.Verify(
            x => x.Send(
                It.IsAny<LoginCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData(null, "123456")]
    [InlineData("", "123456")]
    [InlineData("   ", "123456")]
    [InlineData("ialon@email.com", null)]
    [InlineData("ialon@email.com", "")]
    [InlineData("ialon@email.com", "   ")]
    public async Task
        Login_DadosInvalidos_DeveLancarException(
            string? email,
            string? senha)
    {
        // Arrange

        var request = new LoginRequest(
            email!,
            senha!);

        // Act & Assert

        await Assert.ThrowsAsync<DomainException>(
            () => _controller.Login(request));
    }

    [Fact]
    public async Task
        Login_CredenciaisValidas_DevePassarParametrosCorretos()
    {
        // Arrange

        var request = new LoginRequest(
            "joao@email.com",
            "senha123");

        _mediatorMock
            .Setup(x =>
                x.Send(
                    It.IsAny<LoginCommand>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LoginResponse("token"))
            .Callback(new InvocationAction(invocation =>
            {
                var command = invocation.Arguments[0] as LoginCommand;
                Assert.NotNull(command);
                Assert.Equal(request.Email, command.Email);
                Assert.Equal(request.Senha, command.Senha);
            }));

        // Act

        await _controller.Login(request);

        // Assert

        _mediatorMock.Verify(
            x => x.Send(
                It.IsAny<LoginCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
