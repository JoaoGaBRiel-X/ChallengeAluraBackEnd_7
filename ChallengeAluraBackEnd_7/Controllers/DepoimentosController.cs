using AutoMapper;
using ChallengeAluraBackEnd_7.Data;
using ChallengeAluraBackEnd_7.Data.Dtos;
using ChallengeAluraBackEnd_7.Models;
using ChallengeAluraBackEnd_7.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAluraBackEnd_7.Controllers;

[ApiController]
[Route("[controller]")]
public class DepoimentosController : ControllerBase
{
    private DepoimentoService _depoimentoService;

    public DepoimentosController(DepoimentoService depoimentoService)
    {
        _depoimentoService = depoimentoService;
    }

    [HttpPost]
    public IActionResult AdicionaDepoimento([FromForm] CreateDepoimentoDto depoimentoDto)
    {
        var resultado = _depoimentoService.Adiciona(depoimentoDto);

        if (resultado != null)
        {
            return CreatedAtAction(nameof(RecuperaDepoimentoPorId), new { id = resultado.Id }, resultado);
        }
        return BadRequest();
    }

    [HttpGet]
    public IEnumerable<ReadDepoimentoDto> RecuperaDepoimentos([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _depoimentoService.Recupera(skip, take);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaDepoimentoPorId(int id)
    {
        var depoimentoDto = _depoimentoService.RecuperaPorId(id);
        if (depoimentoDto != null)
        {
            return Ok(depoimentoDto);
        }
        return NotFound();
    }

    [HttpGet("{id}/download")]
    public IActionResult DownloadFoto(int id)
    {
        var depoimento = _depoimentoService.DownloadFoto(id);
        if (depoimento != null && depoimento.Foto != null)
        {
            var fotoDepoimento = System.IO.File.ReadAllBytes(depoimento.Foto);
            return File(fotoDepoimento, "image/jpeg");
        }
        return BadRequest();

    }

    [HttpGet("depoimentos-home")]
    public IEnumerable<ReadDepoimentoDto> DepoimentosHome()
    {
        return _depoimentoService.Home();
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaDepoimento(int id, [FromBody] UpdateDepoimentoDto depoimentoDto)
    {
        var depoimento = _depoimentoService.Atualiza(id, depoimentoDto);
        if (depoimento == null) return NotFound();
        return NoContent();
    }

    //[HttpPatch("{id}")]
    //public IActionResult AtualizaDepoimentoParcial(int id, JsonPatchDocument<UpdateDepoimentoDto> patch)
    //{
    //    var depoimento = _context.Depoimentos.FirstOrDefault(depoimento => depoimento.Id == id);
    //    if (depoimento == null) return NotFound();

    //    var depoimentoParaAtualizar = _mapper.Map<UpdateDepoimentoDto>(depoimento);

    //    patch.ApplyTo(depoimentoParaAtualizar, ModelState);

    //    if (!TryValidateModel(depoimentoParaAtualizar))
    //    {
    //        return ValidationProblem(ModelState);
    //    }
    //    _mapper.Map(depoimentoParaAtualizar, depoimento);
    //    _context.SaveChanges();
    //    return NoContent();
    //}

    [HttpDelete("{id}")]
    public IActionResult DeletaDepoimento(int id)
    {
        var retorno = _depoimentoService.Deleta(id);
        if (retorno == null) return NotFound();
        return NoContent();
    }
}
