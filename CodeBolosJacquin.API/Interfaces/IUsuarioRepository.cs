using CodeBolosJacquin.API.Domains;
using CodeBolosJacquin.API.ViewModels;

namespace CodeBolosJacquin.API.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ValidarEmailESenhaAsync(LoginViewModel login);
    }
}
