using CarShop.Data;
using CarShop.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Controllers
{
    [Route("api/car-classes")]
    [ApiController]
    public class CarClassController : ControllerBase
    {
        private readonly ICarClassRepository _carClassRepository;

        public CarClassController(ICarClassRepository carClassRepository)
        {
            _carClassRepository = carClassRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new ApiResponseDto(() =>
            {
                var carClasses = _carClassRepository.GetAll().ToArray();
                return carClasses.ToList();
            });
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var carClass = new CarClass
                {
                    Name = dto.Name
                };

                return _carClassRepository.Create(carClass);
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                return _carClassRepository.Get(id);
            });
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var carClass = _carClassRepository.Get(id);
                if (carClass == null) throw new Exception("Car class not found");

                carClass.Name = dto.Name;

                return _carClassRepository.Update(carClass);
            });
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                var carClass = _carClassRepository.Get(id);
                if (carClass == null) throw new Exception("Car class not found");

                _carClassRepository.Delete(carClass);
            });
            return Ok(response);
        }
    }
}
