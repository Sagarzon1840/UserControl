namespace Business.Api.Application.DTOs;

public class UsuarioResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public char Sexo { get; set; }
}
