using System.ComponentModel.DataAnnotations;

namespace CodeBolosJacquin.API.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress] // Valida os dados de email
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "A senha é obrigatória")]
        public string Senha { get; set; } = null!;
    }
}
