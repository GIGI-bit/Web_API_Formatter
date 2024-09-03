using Microsoft.AspNetCore.Mvc;
using Web_API_Formatter.DTOs;
using Web_API_Formatter.Entities;
using Web_API_Formatter.Services.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_API_Formatter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/<StudentController>
        [HttpGet]
        public async Task<IEnumerable<StudentDTO>> Get()
        {
            var items = await _studentService.GetAllAsync();
            
            
                var students = items.Select(s =>
                {
                    return new StudentDTO
                    {
                        Fullname = s.Fullname,
                        Score = s.Score,
                        SeriaNo = s.SeriaNo,
                        Id = s.Id,
                        Age = s.Age,
                    };
                });
                return students;
            
            
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var student = await _studentService.GetAsync(p => p.Id == id);
            if (student != null)
            {
                var studentDto = new StudentDTO
                {
                    Id = student.Id,
                    Fullname = student.Fullname,
                    Age = student.Age,
                    Score = student.Score,
                    SeriaNo = student.SeriaNo
                };
                return Ok(studentDto);
            }
            else return NotFound();

        }

        // POST api/<StudentController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StudentAddDTO value)
        {

            var student = new Student
            {
                Fullname = value.Fullname,
                Score = value.Score,
                SeriaNo = value.SeriaNo,
                Age = value.Age,
            };
            await _studentService.AddAsync(student);
                return Ok(student);
            

        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StudentAddDTO value)
        {
            var student = await _studentService.GetAsync(s => s.Id == id);
            if (student != null)
            {
                student.Fullname = value.Fullname;
                student.Score = value.Score;
                student.Age = value.Age;
                student.SeriaNo = value.SeriaNo;
                return Ok(student);
            }
            return BadRequest();
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _studentService.GetAsync(s => s.Id == id);
            if (item != null)
            {
                await _studentService.DeleteAsync(item);
                return Ok(item);

            }
            else return BadRequest();

        }
    }
}
