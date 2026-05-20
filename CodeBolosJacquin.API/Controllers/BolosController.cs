using CodeBolosJacquin.API.Interfaces;
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



    }
}
