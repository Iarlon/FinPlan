using Financeiro.Application.Queries;
using Financeiro.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financeiro.Application.Controllers;

[ApiController]
[Route("categorias")]
public class CategoriaController : ControllerBase
{
    private readonly IMediator _mediator;
    public CategoriaController(IMediator mediator) => _mediator = mediator;
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaResponse>>> ObterCategorias()
    {
        var query = new ObterCategoriasQuery();
        var result = await _mediator.Send(query);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

}
