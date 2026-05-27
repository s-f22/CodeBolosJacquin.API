using CodeBolosJacquin.API.Context;
using CodeBolosJacquin.API.Domains;
using CodeBolosJacquin.API.Interfaces;
using CodeBolosJacquin.API.Utils;
using CodeBolosJacquin.API.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CodeBolosJacquin.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly BolosJacquinContext _context;

        public UsuarioRepository(BolosJacquinContext context)
        {
            _context = context;
        }



        public async Task<Usuario?> ValidarEmailESenhaAsync(LoginViewModel login)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == login.Email);

            if (usuario == null)
                return null;

            if (SenhaUtils.EstaHashada(usuario.Senha))
            {
                if (SenhaUtils.VerificarSenha(login.Senha, usuario.Senha))
                    return usuario;

                return null;
            }
            else
            {
                if (usuario.Senha == login.Senha)
                {
                    usuario.Senha = SenhaUtils.HashSenha(login.Senha);
                    await _context.SaveChangesAsync();
                    return usuario;
                }

                return null;
            }
        }
    }
}
