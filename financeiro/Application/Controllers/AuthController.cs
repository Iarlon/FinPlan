using Financeiro.Application.Command;
using Financeiro.Application.Request;
using Financeiro.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Application.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Senha))
        {
            throw new DomainException("Dados inválidos.");
        }
        var command = new LoginCommand(
            request.Email,
            request.Senha);

        var response =
            await _mediator.Send(command);

        return Ok(response);
    }
}
