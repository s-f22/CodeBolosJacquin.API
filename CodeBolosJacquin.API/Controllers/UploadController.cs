using CodeBolosJacquin.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeBolosJacquin.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _enviroment;

        public UploadController(IWebHostEnvironment enviroment)
        {
            _enviroment = enviroment;
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadRequestViewModel request)
        {
            var file = request.File;

            if (file == null || file.Length == 0)
                return BadRequest("Arquivo não informado");

            var uploadsFolder = Path.Combine(_enviroment.ContentRootPath, "Uploads");

            Directory.CreateDirectory(uploadsFolder);

            var fileName = Path.GetFileName(file.FileName);

            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var fileStream = System.IO.File.Create(filePath);

            await file.CopyToAsync(fileStream);

            var relativePath = $"Uploads/{fileName}";

            return Ok(new
            {
                CaminhoImagem = relativePath
            });

        }

    }
}
