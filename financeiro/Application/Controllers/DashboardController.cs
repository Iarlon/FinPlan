using Financeiro.Application.Queries;
using Financeiro.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Application.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("resumo-saldo")]
    [ProducesResponseType(typeof(ResumoSaldoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterResumoSaldo(CancellationToken cancellationToken)
    {
        var usuarioIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(usuarioIdClaim) || !long.TryParse(usuarioIdClaim, out var usuarioId))
        {
            return Unauthorized("Usuário não identificado no token de autenticação.");
        }
        var query = new ObterResumoSaldoQuery(usuarioId);

        var resultado = await _mediator.Send(query, cancellationToken);

        return Ok(resultado);
    }
}
