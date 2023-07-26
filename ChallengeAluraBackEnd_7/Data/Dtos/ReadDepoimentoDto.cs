using System.ComponentModel.DataAnnotations;

namespace ChallengeAluraBackEnd_7.Data.Dtos;

public class ReadDepoimentoDto
{
    public int Id { get; set; }
    public string Foto { get; set; }
    public string TextoDepoimento { get; set; }
    public string NomeDaPessoa { get; set; }
}
