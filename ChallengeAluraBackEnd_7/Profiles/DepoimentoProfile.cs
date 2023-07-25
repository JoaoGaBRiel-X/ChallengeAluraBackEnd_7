using AutoMapper;
using ChallengeAluraBackEnd_7.Data.Dtos;
using ChallengeAluraBackEnd_7.Models;

namespace ChallengeAluraBackEnd_7.Profiles;

public class DepoimentoProfile : Profile
{
    public DepoimentoProfile()
    {
        CreateMap<CreateDepoimentoDto, Depoimento>();
    }
}
