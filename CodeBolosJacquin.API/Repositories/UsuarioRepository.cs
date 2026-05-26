using CodeBolosJacquin.API.Context;
using CodeBolosJacquin.API.Domains;
using CodeBolosJacquin.API.Interfaces;
using CodeBolosJacquin.API.ViewModels;

namespace CodeBolosJacquin.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly BolosJacquinContext _context;

        public UsuarioRepository(BolosJacquinContext context)
        {
            _context = context;
        }



        public Task<Usuario?> ValidarEmailESenhaAsync(LoginViewModel login)
        {
            throw new NotImplementedException();
        }
    }
}
