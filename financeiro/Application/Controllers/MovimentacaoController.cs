using Financeiro.Application.Command;
using Financeiro.Application.Queries;
using Financeiro.Application.Request;
using Financeiro.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Application.Controllers;

[ApiController]
[Route("movimentacoes")]
public class MovimentacaoController : ControllerBase
{

    private readonly IMediator _mediator;
    public MovimentacaoController(IMediator mediator) => _mediator = mediator;
    
    [HttpPost]
    public async Task<ActionResult> Criar([FromBody] CriarMovimentacaoRequest request)
    {
        var usuarioId = 1; // depois virá do JWT
        var orcamentoId = 1; // depois virá do contexto/regra

        var command = new CriarMovimentacaoCommand(
            orcamentoId,
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
}
