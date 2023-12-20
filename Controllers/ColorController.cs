using CarShop.Data;
using CarShop.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Controllers
{
    [Route("api/colors")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColorRepository _colorRepository;

        public ColorController(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new ApiResponseDto(() =>
            {
                var colors = _colorRepository.GetAll().ToArray();
                return colors.ToList();
            });
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var color = new Color
                {
                    Name = dto.Name
                };

                return _colorRepository.Create(color);
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                return _colorRepository.Get(id);
            });
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var color = _colorRepository.Get(id);
                if (color == null) throw new Exception("Color not found");

                color.Name = dto.Name;

                return _colorRepository.Update(color);
            });
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                var color = _colorRepository.Get(id);
                if (color == null) throw new Exception("Color not found");

                _colorRepository.Delete(color);
            });
            return Ok(response);
        }
    }
}
