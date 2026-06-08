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
            request.Descricao,
            request.Tag,
            request.DataMovimentacao,
            request.CategoriaId);

        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(ObterPorId), new { id }, null);
    }
    [HttpGet]
    public async Task<ActionResult<PagedResponse<MovimentacaoHistoricoResponse>>> ObterMovimentacoes(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] long? categoriaId = null,
    [FromQuery] DateTime? dataInicio = null,
    [FromQuery] DateTime? dataFim = null,
    [FromQuery] int? tipoMovimentacao = null,
    [FromQuery] string? tag = null)
    {
        var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(usuarioIdClaim, out var usuarioId))
            return Unauthorized();

        var query = new ObterHistoricoMovimentacaoQuery(
            usuarioId,
            pageNumber,
            pageSize,
            categoriaId,
            dataInicio ?? default,
            dataFim ?? default,
            tipoMovimentacao,
            tag
        );

        var result = await _mediator.Send(query);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("recentes")]
    public async Task<ActionResult<MovimentacaoRecentesResponse>> ObterMovimentacoesRecentes()
    {
        var usuarioIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(usuarioIdClaim, out var usuarioId))
            return Unauthorized();
        var result = await _mediator.Send(new ObterMovimentacaoRecenteQuery(usuarioId));

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<MovimentacaoResponse>> ObterPorId(long id)
    {
        var result = await _mediator.Send(new ObterMovimentacaoPorIdQuery(id));

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("categorias")]
    public async Task<ActionResult<MovimentacaoPorCategoriaResponse>> ObterMovimentacoesPorCategoria()
    {
        var usuarioIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(usuarioIdClaim, out var usuarioId))
            return Unauthorized();
        var result = await _mediator.Send(new ObterMovimentacaoPorCategoriaQuery(usuarioId));

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
