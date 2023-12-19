using CarShop.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        public ImageController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public class FileUploadApi
        {
            public IFormFile files { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] FileUploadApi objFile)
        {
            try
            {
                if (objFile.files.Length == 0) throw new Exception("No image source found");

                string uploadFolder = "\\uploads\\";
                string uploadDir = _environment.WebRootPath + uploadFolder;

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                using (FileStream fileStream = System.IO.File.Create(uploadDir + objFile.files.FileName))
                {
                    objFile.files.CopyTo(fileStream);
                    fileStream.Flush();
                    return Ok(uploadFolder + objFile.files.FileName);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }
    }
}
