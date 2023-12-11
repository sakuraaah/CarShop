using CarShop.Data;
using CarShop.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Controllers
{
    [Route("api/marks")]
    [ApiController]
    public class MarkController : ControllerBase
    {
        private readonly IMarkRepository _markRepository;

        public MarkController(IMarkRepository markRepository)
        {
            _markRepository = markRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new ApiResponseDto(() =>
            {
                var marks = _markRepository.GetAll().ToArray();
                return marks.ToList();
            });
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var mark = new Mark
                {
                    Name = dto.Name
                };

                return _markRepository.Create(mark);
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                return _markRepository.Get(id);
            });
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var mark = _markRepository.Get(id);
                if (mark == null) throw new Exception("Marka nav atrasta");

                mark.Name = dto.Name;

                return _markRepository.Update(mark);
            });
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                var mark = _markRepository.Get(id);
                if (mark == null) throw new Exception("Marka nav atrasta");

                _markRepository.Delete(mark);
            });
            return Ok(response);
        }
    }
}
