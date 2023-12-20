using CarShop.Data;
using CarShop.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Controllers
{
    [Route("api/rent-categories")]
    [ApiController]
    public class RentCategoryController : ControllerBase
    {
        private readonly IRentCategoryRepository _rentCategoryRepository;

        public RentCategoryController(IRentCategoryRepository rentCategoryRepository)
        {
            _rentCategoryRepository = rentCategoryRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new ApiResponseDto(() =>
            {
                var rentCategories = _rentCategoryRepository.GetAll().ToArray();
                return rentCategories.ToList();
            });
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var rentCategory = new RentCategory
                {
                    Name = dto.Name
                };

                return _rentCategoryRepository.Create(rentCategory);
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                return _rentCategoryRepository.Get(id);
            });
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var rentCategory = _rentCategoryRepository.Get(id);
                if (rentCategory == null) throw new Exception("Rent category not found");

                rentCategory.Name = dto.Name;

                return _rentCategoryRepository.Update(rentCategory);
            });
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                var rentCategory = _rentCategoryRepository.Get(id);
                if (rentCategory == null) throw new Exception("Rent category not found");

                _rentCategoryRepository.Delete(rentCategory);
            });
            return Ok(response);
        }
    }
}
