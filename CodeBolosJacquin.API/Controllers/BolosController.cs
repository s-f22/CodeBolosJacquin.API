using CodeBolosJacquin.API.Interfaces;
using CodeBolosJacquin.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeBolosJacquin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BolosController : ControllerBase
    {
        private readonly IBoloRepository _boloRepository;

        public BolosController(IBoloRepository boloRepository)
        {
            _boloRepository = boloRepository;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var bolos = await _boloRepository.ListarTodosAsync();
                return Ok(bolos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao listar os bolos", erro = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var bolo = await _boloRepository.BuscarPorIdAsync(id);

                if (bolo == null)
                    return NotFound(new { mensagem = "Bolo não encontrado" });

                return Ok(bolo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao obter o bolo", erro = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, BoloRequestViewModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var resultado = await _boloRepository.AtualizarAsync(id, request);

                if (!resultado)
                    return NotFound(new { mensagem = "Bolo não encontrado" });

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {mensagem = ex.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {mensagem = "Erro ao atualizar o bolo", erro = ex.Message});
            }

        }



    }
}
