using AutoMapper;
using ChallengeAluraBackEnd_7.Data;
using ChallengeAluraBackEnd_7.Data.Dtos;
using ChallengeAluraBackEnd_7.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;

namespace ChallengeAluraBackEnd_7.Services;

public class DepoimentoService
{
    private IMapper _mapper;
    private DepoimentosContext _context;

    public DepoimentoService(IMapper mapper, DepoimentosContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public Depoimento Adiciona(CreateDepoimentoDto dto)
    {
        var filePath = Path.Combine("Storage", dto.Foto != null ? dto.Foto.FileName : "semFoto.jpg");
        if (dto.Foto != null)
        {
            using Stream fileStream = new FileStream(filePath, FileMode.Create);
            dto.Foto.CopyTo(fileStream);
        }

        Depoimento depoimento = _mapper.Map<Depoimento>(dto);

        depoimento.Foto = filePath;

        _context.Depoimentos.Add(depoimento);
        _context.SaveChanges();

        return depoimento;
    }

    public List<ReadDepoimentoDto> Recupera(int skip, int take)
    {
        var listaDepoimentos = _mapper.Map<List<ReadDepoimentoDto>>(_context.Depoimentos.Skip(skip).Take(take));
        foreach (var depoimento in listaDepoimentos)
        {
            if(depoimento.Foto != null)
            {
                depoimento.Foto = $"/depoimentos/{depoimento.Id}/download";
            }
        }
        return listaDepoimentos;
    }

    public Depoimento? Atualiza(int id, UpdateDepoimentoDto depoimentoDto)
    {
        var depoimento = _context.Depoimentos.FirstOrDefault(depoimento => depoimento.Id == id);
        if (depoimento == null) return depoimento;
        _mapper.Map(depoimentoDto, depoimento);
        _context.SaveChanges();
        return depoimento;
    }

    public Depoimento? DownloadFoto(int id)
    {
        return _context.Depoimentos.FirstOrDefault(depoimento => depoimento.Id == id);
    }

    public IEnumerable<ReadDepoimentoDto> Home()
    {
        var listaDepoimentos = _mapper.Map<List<ReadDepoimentoDto>>(_context.Depoimentos.FromSqlRaw("SELECT Id, Foto, TextoDepoimento, NomeDaPessoa FROM depoimentos ORDER BY RAND() LIMIT 3").ToList());
        foreach (var depoimento in listaDepoimentos)
        {
            if (depoimento.Foto != null)
            {
                depoimento.Foto = $"/depoimentos/{depoimento.Id}/download";
            }
        }
        return listaDepoimentos;
    }

    public ReadDepoimentoDto RecuperaPorId(int id)
    {
        var depoimento = _context.Depoimentos.FirstOrDefault(depoimento => depoimento.Id == id);
        var retorno = _mapper.Map<ReadDepoimentoDto>(depoimento);
        if (retorno.Foto != null)
        {
            retorno.Foto = $"/depoimentos/{id}/download";
        }
        return retorno;
    }

    internal Depoimento? Deleta(int id)
    {
        var depoimento = _context.Depoimentos.FirstOrDefault(depoimento => depoimento.Id == id);
        if (depoimento == null) return depoimento;
        _context.Remove(depoimento);
        _context.SaveChanges();
        return depoimento;
    }
}
