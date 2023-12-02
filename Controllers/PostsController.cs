using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpPost]
        public object Create(object model)
        {

        }

        [HttpPatch]
        public object Update(object model)
        {

        }

        [HttpGet("{id}")]
        public object Get(int id)
        {

        }

        [HttpGet]
        public object GetAll()
        {

        }

        [HttpDelete("{id}")]
        public object Delete(int id)
        {

        }
    }
}
