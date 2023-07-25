using ChallengeAluraBackEnd_7.Data;
using ChallengeAluraBackEnd_7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace ChallengeAluraBackEnd_7.Controllers;

[ApiController]
[Route("[controller]")]
public class DepoimentosController : ControllerBase
{
    private DepoimentosContext _context;

    public DepoimentosController(DepoimentosContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AdicionaDepoimento([FromBody] Depoimento depoimento)
    {
        _context.Depoimentos.Add(depoimento);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaDepoimentoPorId), new { id = depoimento.Id }, depoimento);
    }

    [HttpGet]
    public IEnumerable<Depoimento> RecuperaDepoimentos([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _context.Depoimentos.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaDepoimentoPorId(int id)
    {
        var depoimento = _context.Depoimentos.FirstOrDefault(depoimento => depoimento.Id == id);
        if (depoimento == null) return NotFound();
        return Ok(depoimento);
    }
}
