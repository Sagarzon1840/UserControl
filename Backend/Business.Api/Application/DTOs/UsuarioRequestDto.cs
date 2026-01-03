using System.ComponentModel.DataAnnotations;

namespace Business.Api.Application.DTOs;

public class UsuarioRequestDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
    public DateTime FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es requerido")]
    [RegularExpression("^[MF]$", ErrorMessage = "El sexo debe ser 'M' o 'F'")]
    public char Sexo { get; set; }
}
