using AutoMapper;
using ChallengeAluraBackEnd_7.Data;
using ChallengeAluraBackEnd_7.Data.Dtos;
using ChallengeAluraBackEnd_7.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAluraBackEnd_7.Controllers;

[ApiController]
[Route("[controller]")]
public class DepoimentosController : ControllerBase
{
    private DepoimentosContext _context;
    private IMapper _mapper;

    public DepoimentosController(DepoimentosContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AdicionaDepoimento([FromForm] CreateDepoimentoDto depoimentoDto)
    {

        var filePath = Path.Combine("Storage", depoimentoDto.Foto.FileName);
        if (depoimentoDto.Foto != null) {
            using Stream fileStream = new FileStream(filePath, FileMode.Create);
            depoimentoDto.Foto.CopyTo(fileStream);
        }

        Depoimento depoimento = _mapper.Map<Depoimento>(depoimentoDto);

        if (depoimentoDto != null)
        {
            depoimento.Foto = filePath;
        } 
        else
        {
            depoimento.Foto = null;
        }

        _context.Depoimentos.Add(depoimento);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaDepoimentoPorId), new { id = depoimento.Id }, depoimento);
    }

    [HttpGet]
    public IEnumerable<ReadDepoimentoDto> RecuperaDepoimentos([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadDepoimentoDto>>(_context.Depoimentos.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaDepoimentoPorId(int id)
    {
        var depoimento = _context.Depoimentos.FirstOrDefault(depoimento => depoimento.Id == id);
        if (depoimento == null) return NotFound();
        var depoimentoDto = _mapper.Map<ReadDepoimentoDto>(depoimento);
        return Ok(depoimentoDto);
    }

    [HttpGet("{id}/download")]
    public IActionResult DownloadFoto(int id)
    {
        var depoimento = _context.Depoimentos.FirstOrDefault(depoimento =>depoimento.Id == id);
        if (depoimento == null) return BadRequest();

        var fotoDepoimento = System.IO.File.ReadAllBytes(depoimento.Foto);

        return File(fotoDepoimento, "image/jpeg");
    }

    [HttpGet("depoimentos-home")]
    public IEnumerable<ReadDepoimentoDto> DepoimentosHome()
    {
        return _mapper.Map<List<ReadDepoimentoDto>>(_context.Depoimentos.FromSqlRaw("SELECT Id, Foto, TextoDepoimento, NomeDaPessoa FROM depoimentos ORDER BY RAND() LIMIT 3").ToList());
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaDepoimento(int id, [FromBody] UpdateDepoimentoDto depoimentoDto)
    {
        var depoimento = _context.Depoimentos.FirstOrDefault(depoimento => depoimento.Id == id);
        if (depoimento == null) return NotFound();
        _mapper.Map(depoimentoDto, depoimento);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaDepoimentoParcial(int id, JsonPatchDocument<UpdateDepoimentoDto> patch)
    {
        var depoimento = _context.Depoimentos.FirstOrDefault(depoimento => depoimento.Id == id);
        if (depoimento == null) return NotFound();

        var depoimentoParaAtualizar = _mapper.Map<UpdateDepoimentoDto>(depoimento);

        patch.ApplyTo(depoimentoParaAtualizar, ModelState);

        if (!TryValidateModel(depoimentoParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(depoimentoParaAtualizar, depoimento);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaDepoimento(int id)
    {
        var depoimento = _context.Depoimentos.FirstOrDefault(depoimento => depoimento.Id == id);
        if(depoimento == null) return NotFound();
        _context.Remove(depoimento);
        _context.SaveChanges();
        return NoContent();
    }
}
