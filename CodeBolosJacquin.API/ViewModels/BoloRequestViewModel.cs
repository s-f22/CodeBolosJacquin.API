using System.ComponentModel.DataAnnotations;

namespace CodeBolosJacquin.API.ViewModels
{
    public class BoloRequestViewModel
    {
        [Required(ErrorMessage = "O nome do bolo é obrigatório")]
        public required string Nome { get; set; }
        public string? Descricao { get; set; }
        [Required(ErrorMessage = "O preço do bolo é obrigatório")]
        public required decimal Preco { get; set; }
        public decimal? Peso { get; set; }
        [Required(ErrorMessage = "Insira ao menos uma categoria")]
        public required List<string> Categorias { get; set; } = new();
        public List<string>? Imagens { get; set; } = new();
    }
}
