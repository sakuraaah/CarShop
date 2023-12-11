using CarShop.Data;
using CarShop.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Controllers
{
    [Route("api/body-types")]
    [ApiController]
    public class BodyTypeController : ControllerBase
    {
        private readonly IBodyTypeRepository _bodyTypeRepository;

        public BodyTypeController(IBodyTypeRepository bodyTypeRepository)
        {
            _bodyTypeRepository = bodyTypeRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new ApiResponseDto(() =>
            {
                var bodyTypes = _bodyTypeRepository.GetAll().ToArray();
                return bodyTypes.ToList();
            });
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var bodyType = new BodyType
                {
                    Name = dto.Name
                };

                return _bodyTypeRepository.Create(bodyType);
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                return _bodyTypeRepository.Get(id);
            });
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var bodyType = _bodyTypeRepository.Get(id);
                if (bodyType == null) throw new Exception("Virsbūves tips nav atrasts");

                bodyType.Name = dto.Name;

                return _bodyTypeRepository.Update(bodyType);
            });
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                var bodyType = _bodyTypeRepository.Get(id);
                if (bodyType == null) throw new Exception("Virsbūves tips nav atrasts");

                _bodyTypeRepository.Delete(bodyType);
            });
            return Ok(response);
        }
    }
}
