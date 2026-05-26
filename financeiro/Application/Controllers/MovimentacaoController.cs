using Financeiro.Application.Command;
using Financeiro.Application.Queries;
using Financeiro.Application.Request;
using Financeiro.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Financeiro.Application.Controllers;

[Authorize]
[ApiController]
[Route("movimentacoes")]
public class MovimentacaoController : ControllerBase
{

    private readonly IMediator _mediator;
    public MovimentacaoController(IMediator mediator) => _mediator = mediator;
    
    [HttpPost]
    public async Task<ActionResult> Criar([FromBody] CriarMovimentacaoRequest request)
    {
         var usuarioIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(usuarioIdClaim, out var usuarioId))
            return Unauthorized();

        var command = new CriarMovimentacaoCommand(
            usuarioId,
            request.Valor,
            request.Tipo,
            request.Descricao,
            request.Tag,
            request.DataMovimentacao,
            request.CategoriaId);

        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(ObterPorId), new { id }, null);
    }
    [HttpGet("{id:long}")]
    public async Task<ActionResult<MovimentacaoResponse>> ObterPorId(long id)
    {
        var result = await _mediator.Send(new ObterMovimentacaoPorIdQuery(id));

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<MovimentacaoPorCategoriaResponse>> ObterMovimentacoesPorCategoria()
    {
        return null;
    }
}
