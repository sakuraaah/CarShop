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
        private readonly string _uploadsDirectory = "uploads";
        public ImageController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public class FileUploadApi
        {
            public IFormFile imgSrc { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] FileUploadApi objFile)
        {
            var response = new ApiResponseDto(() =>
            {
                if (objFile.imgSrc.Length == 0) throw new Exception("No image source found");

                string uploadDir = Path.Combine(_environment.WebRootPath, _uploadsDirectory);

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                using (FileStream fileStream = System.IO.File.Create(uploadDir + "\\" + objFile.imgSrc.FileName))
                {
                    objFile.imgSrc.CopyTo(fileStream);
                    fileStream.Flush();
                    return Ok("api/image/" + objFile.imgSrc.FileName);
                }
            });
            return Ok(response);
        }

        [HttpGet("{imageName}")]
        public IActionResult Get(string imageName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imageName)) throw new Exception("No image source found");

                string imgFolder = Path.Combine(_uploadsDirectory, imageName);
                string imgPath = Path.Combine(_environment.WebRootPath, imgFolder);

                if (!System.IO.File.Exists(imgPath))
                {
                    return NotFound();
                }

                var imageFileStream = System.IO.File.OpenRead(imgPath);
                return File(imageFileStream, "image/jpeg");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }
        }
    }
}
