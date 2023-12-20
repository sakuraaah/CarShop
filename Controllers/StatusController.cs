using CarShop.Data;
using CarShop.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Controllers
{
    [Route("api/statuses")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusRepository _statusRepository;

        public StatusController(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new ApiResponseDto(() =>
            {
                var statuses = _statusRepository.GetAll().ToArray();
                return statuses.ToList();
            });
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var status = new Status
                {
                    Name = dto.Name
                };

                return _statusRepository.Create(status);
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                return _statusRepository.Get(id);
            });
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var status = _statusRepository.Get(id);
                if (status == null) throw new Exception("Status not found");

                status.Name = dto.Name;

                return _statusRepository.Update(status);
            });
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                var status = _statusRepository.Get(id);
                if (status == null) throw new Exception("Status not found");

                _statusRepository.Delete(status);
            });
            return Ok(response);
        }
    }
}
