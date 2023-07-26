using System.ComponentModel.DataAnnotations;

namespace ChallengeAluraBackEnd_7.Data.Dtos;

public class UpdateDepoimentoDto
{
    public string Foto { get; set; }
    [Required(ErrorMessage = "O texto do depoimento é obrigatório!")]
    [MaxLength(255, ErrorMessage = "O tamanho do depoimento nao pode exceder 255 caracteres")]
    public string TextoDepoimento { get; set; }
    [Required(ErrorMessage = "Nome da pessoa é obrigatório")]
    [MaxLength(60, ErrorMessage = "O nome não pode exceder 60 caracteres")]
    public string NomeDaPessoa { get; set; }
}
