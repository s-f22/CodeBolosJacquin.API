namespace CodeBolosJacquin.API.ViewModels
{
    public class BoloResponseViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public decimal? Peso { get; set; }
        public List<string> Categorias { get; set; } = new();
        public List<string> Imagens { get; set; } = new();
    }
}
