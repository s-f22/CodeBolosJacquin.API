using CodeBolosJacquin.API.Context;
using CodeBolosJacquin.API.Interfaces;
using CodeBolosJacquin.API.ViewModels;

namespace CodeBolosJacquin.API.Repositories
{
    public class BoloRepository : IBoloRepository
    {
        // Injetando BolosJacquinContext com um método construtor
        private readonly BolosJacquinContext _context;

        public BoloRepository(BolosJacquinContext context)
        {
            _context = context;
        }




        public Task<bool> AtualizarAsync(int id, BoloRequestViewModel bolo)
        {
            throw new NotImplementedException();
        }



        public Task<BoloResponseViewModel?> BuscarPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }



        public Task<BoloResponseViewModel> CadastrarAsync(BoloRequestViewModel bolo)
        {
            throw new NotImplementedException();
        }



        public Task<IEnumerable<BoloResponseViewModel>> ListarTodosAsync()
        {
            throw new NotImplementedException();
        }



        public Task<bool> RemoverAsync(int id)
        {
            throw new NotImplementedException();
        }


    }
}
