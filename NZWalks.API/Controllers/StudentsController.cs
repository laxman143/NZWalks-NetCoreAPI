using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // GET https://localhost:portnumber/api/students
        [HttpGet]
        public IActionResult GetAllStudents() {
            string[] studentNames = new string[] { "John", "Laxman" };
            return Ok(studentNames);
        }
    }
}
