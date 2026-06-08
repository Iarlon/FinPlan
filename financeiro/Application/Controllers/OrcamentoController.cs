using Financeiro.Application.Queries;
using Financeiro.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Financeiro.Application.Controllers;

[Authorize]
[ApiController]
[Route("orcamento")]

public class OrcamentoController : ControllerBase
{
    private readonly IMediator _mediator;
    public OrcamentoController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<MovimentacaoPorCategoriaResponse>> ObterSaldo()
    {
        var usuarioIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(usuarioIdClaim, out var usuarioId))
            return Unauthorized();

        var query = new ObterSaldoQuery(usuarioId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("/tendencia")]
    public async Task<ActionResult<ResumoSaldoResponse>> ObterResumoSaldo()
    {
        var usuarioIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(usuarioIdClaim, out var usuarioId))
            return Unauthorized();
        var query = new ObterResumoSaldoQuery(usuarioId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
