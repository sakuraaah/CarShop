using CarShop.Data;
using CarShop.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Controllers
{
    [Route("api/features")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureRepository _featureRepository;

        public FeatureController(IFeatureRepository featureRepository)
        {
            _featureRepository = featureRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = new ApiResponseDto(() =>
            {
                var features = _featureRepository.GetAll().ToArray();
                return features.ToList();
            });
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var feature = new Feature
                {
                    Name = dto.Name
                };

                return _featureRepository.Create(feature);
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                return _featureRepository.Get(id);
            });
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] NameDto dto)
        {
            var response = new ApiResponseDto(() =>
            {
                var feature = _featureRepository.Get(id);
                if (feature == null) throw new Exception("Feature not found");

                feature.Name = dto.Name;

                return _featureRepository.Update(feature);
            });
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = new ApiResponseDto(() =>
            {
                var feature = _featureRepository.Get(id);
                if (feature == null) throw new Exception("Feature not found");

                _featureRepository.Delete(feature);
            });
            return Ok(response);
        }
    }
}
