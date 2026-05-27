using CodeBolosJacquin.API.Interfaces;
using CodeBolosJacquin.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodeBolosJacquin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public LoginsController(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var usuarioBuscado = await _usuarioRepository.ValidarEmailESenhaAsync(login);

                if (usuarioBuscado == null)
                    return NotFound("Email ou senha inválidos");

                var papel = "Administrador";

                var minhasClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, usuarioBuscado.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, usuarioBuscado.Id.ToString()),
                    new Claim(ClaimTypes.Role, papel),
                    new Claim("role", papel)
                };

                var secretKey = _configuration["Jwt:Key"];
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];

                if (
                    string.IsNullOrWhiteSpace(secretKey) ||
                    string.IsNullOrWhiteSpace(issuer) ||
                    string.IsNullOrWhiteSpace(audience) 
                   )
                {
                    return StatusCode(500, "Configuração JWT ausente ou inválida");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var meuToken = new JwtSecurityToken
                (
                    issuer: issuer,
                    audience: audience,
                    claims: minhasClaims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(meuToken),
                    Role = papel
                });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



    }
}
