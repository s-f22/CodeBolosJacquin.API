using CodeBolosJacquin.API.Context;
using CodeBolosJacquin.API.Domains;
using CodeBolosJacquin.API.Interfaces;
using CodeBolosJacquin.API.ViewModels;
using Microsoft.EntityFrameworkCore;

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




        public async Task<bool> AtualizarAsync(int id, BoloRequestViewModel boloAtualizado)
        {
            var bolo = await _context.Bolos
                .Include(b => b.Categoria)
                .Include(b => b.BoloImagens)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bolo == null)
                throw new KeyNotFoundException("Bolo não encontrado");

            bolo.Nome = boloAtualizado.Nome ?? bolo.Nome;
            bolo.Descricao = boloAtualizado.Descricao;
            bolo.Preco = boloAtualizado.Preco;
            bolo.Peso = boloAtualizado.Peso;

            bolo.Categoria.Clear();

            var categorias = await ObterCategoriasAsync(boloAtualizado.Categorias);

            foreach (var categoria in categorias)
            {
                bolo.Categoria.Add(categoria);
            }

            if (bolo.BoloImagens.Any())
            {
                _context.BoloImagens.RemoveRange(bolo.BoloImagens);
                bolo.BoloImagens.Clear();
            }

            if (boloAtualizado.Imagens != null)
            {
                foreach (var imagem in boloAtualizado.Imagens)
                {
                    bolo.BoloImagens.Add(new BoloImagen
                    {
                        CaminhoImagem = imagem,
                    });
                }
            }

            await _context.SaveChangesAsync();

            return true;

        }



        public async Task<BoloResponseViewModel?> BuscarPorIdAsync(int id)
        {
            var bolo = await _context.Bolos
                .Include(b => b.Categoria)
                .Include(b => b.BoloImagens)
                .FirstOrDefaultAsync(b => b.Id == id);

            return bolo == null ? null : MapToResponse(bolo);
        }



        public async Task<BoloResponseViewModel> CadastrarAsync(BoloRequestViewModel bolo)
        {
            var novoBolo = new Bolo
            {
                Nome = bolo.Nome,
                Descricao = bolo.Descricao,
                Preco = bolo.Preco,
                Peso = bolo.Peso
            };

            novoBolo.Categoria = await ObterCategoriasAsync(bolo.Categorias);
            novoBolo.BoloImagens = bolo.Imagens?
                .Select(img => new BoloImagen { CaminhoImagem = img})
                .ToList() ?? new List<BoloImagen>();

            _context.Bolos.Add(novoBolo);
            await _context.SaveChangesAsync();

            return MapToResponse(novoBolo);

        }



        public async Task<IEnumerable<BoloResponseViewModel>> ListarTodosAsync()
        {
            var bolos = await _context.Bolos
                .Include(b => b.Categoria)
                .Include(b => b.BoloImagens)
                .ToListAsync();

            return bolos.Select(MapToResponse);
        }



        public async Task<bool> RemoverAsync(int id)
        {
            var bolo = await _context.Bolos
                .Include(b => b.Categoria)
                .Include(b => b.BoloImagens)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bolo == null)
                throw new KeyNotFoundException("Bolo não encontrado");

            if (bolo.BoloImagens.Any())
                _context.BoloImagens.RemoveRange(bolo.BoloImagens);

            if (bolo.Categoria.Any())
                bolo.Categoria.Clear();

            _context.Bolos.Remove(bolo);
            await _context.SaveChangesAsync();

            return true;
        }



        private static BoloResponseViewModel MapToResponse(Bolo bolo)
        {
            return new BoloResponseViewModel 
            { 
                Id = bolo.Id,
                Nome = bolo.Nome,
                Descricao = bolo.Descricao,
                Preco = bolo.Preco,
                Peso = bolo.Peso,
                Categorias = bolo.Categoria.Select(c => c.Nome).ToList(),
                Imagens = bolo.BoloImagens.Select(i => i.CaminhoImagem).ToList(),
            };
        }


        private async Task<List<Categoria>> ObterCategoriasAsync(IEnumerable<string>? categorias)
        {
            var lista = new List<Categoria>();

            if (categorias == null)
                return lista;

            foreach (var nome in categorias
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => n.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
            )
            {
                var categoria = await _context.Categorias
                    .FirstOrDefaultAsync(c => c.Nome == nome);

                if (categoria == null)
                {
                    categoria = new Categoria
                    {
                        Nome = nome,
                    };
                    _context.Categorias.Add(categoria);
                }

                lista.Add(categoria);
            }

            return lista;

        }



    }
}
