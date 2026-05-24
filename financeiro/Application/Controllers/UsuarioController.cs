using financeiro.Domain.Request;
using Financeiro.Application.Command;
using Financeiro.Application.Handles;
using Financeiro.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace financeiro.Application.Controllers;

[ApiController]
[Route("usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuarioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioRequest request)
    {
        var command = new CriarUsuarioCommand(
            request.Nome,
            request.Email,
            request.Senha);

        await _mediator.Send(command);

        return StatusCode(201, new
        {
            Mensagem = "Usuário criado com sucesso!"
        });
    }

}
