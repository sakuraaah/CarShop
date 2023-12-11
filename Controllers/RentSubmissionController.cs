using CarShop.Data;
using CarShop.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Controllers
{
    [Route("api/rent-submissions")]
    [ApiController]
    public class RentSubmissionController : ControllerBase
    {
        private readonly IRentSubmissionRepository _rentSubmissionRepository;

        public RentSubmissionController(IRentSubmissionRepository rentSubmissionRepository)
        {
            _rentSubmissionRepository = rentSubmissionRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new ApiResponseDto(() =>
            {
                var rentSubmissions = _rentSubmissionRepository.GetAll().ToArray();
                return rentSubmissions.ToList();
            });
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] RentSubmissionDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var rentSubmission = new RentSubmission
                {
                    AplNr = dto.AplNr,
                    RegNr = dto.RegNr,
                    CategoryId = dto.CategoryId,
                    MarkId = dto.MarkId,
                    Model = dto.Model,
                    Mileage = dto.Mileage,
                    Year = dto.Year,
                };

                return _rentSubmissionRepository.Create(rentSubmission);
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                return _rentSubmissionRepository.Get(id);
            });
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] RentSubmissionDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var rentSubmission = _rentSubmissionRepository.Get(id);
                if (rentSubmission == null) throw new Exception("Iesniegums nav atrasts");

                rentSubmission.AplNr = dto.AplNr;
                rentSubmission.RegNr = dto.RegNr;
                rentSubmission.CategoryId = dto.CategoryId;
                rentSubmission.MarkId = dto.MarkId;
                rentSubmission.Model = dto.Model;
                rentSubmission.Mileage = dto.Mileage;
                rentSubmission.Year = dto.Year;

                return _rentSubmissionRepository.Update(rentSubmission);
            });
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                var rentSubmission = _rentSubmissionRepository.Get(id);
                if (rentSubmission == null) throw new Exception("Iesniegums nav atrasts");

                _rentSubmissionRepository.Delete(rentSubmission);
            });
            return Ok(response);
        }
    }
}
