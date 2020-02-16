using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models;
using SchoolAPI.Models.DataManager;
using SchoolAPI.Models.DTO;
using SchoolAPI.Models.Repository;

namespace SchoolAPI.Controllers
{
    [Route("api/Students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDataManager _dataRepository;

        public StudentController(IDataRepository<Student, StudentDTO, StudentReceivableDTO> dataRepository)
        {
            _dataRepository = (StudentDataManager)dataRepository;
        }


        // GET: api/Students
        [HttpGet]
        public IActionResult Get()
        {
            var students = _dataRepository.GetAllDtos();
            return Ok(students);
        }

        // GET: api/Students/5
        [HttpGet("{id}", Name = "GetStudent")]
        public IActionResult Get(int id)
        {
            if (!_dataRepository.Exists(id))
            {
                return NotFound("Student not found.");
            }
            var student = _dataRepository.GetDto(id);

            return Ok(student);
        }

        // POST: api/Students
        [HttpPost]
        public IActionResult Post([FromBody] StudentReceivableDTO student)
        {
            if (student is null)
            {
                return BadRequest("Student is null.");
            }
            if (string.IsNullOrEmpty(student.FirstName))
            {
                return BadRequest("First Name is required.");
            }
            if (string.IsNullOrEmpty(student.LastName))
            {
                return BadRequest("Last Name is required.");
            }
            if (string.IsNullOrEmpty(student.Gender))
            {
                return BadRequest("Gender is required.");
            }
            if (student.BirthDay == null)
            {
                return BadRequest("Birthday is required.");
            }
            if (string.IsNullOrEmpty(student.StudentId))
            {
                return BadRequest("Student  ID is required.");
            }
            if (student.Classes != null && student.Classes.Any() && !_dataRepository.AreValidClasses(student.Classes.ToList()))
            {
                return BadRequest("Invalid Classes");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _dataRepository.Add(student);
            return CreatedAtRoute("GetStudent", new { Id = student.Id }, null);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] StudentReceivableDTO student)
        {
            if (!_dataRepository.Exists(id))
            {
                return NotFound("Student not found.");
            }
            if (student is null)
            {
                return BadRequest("Student is null.");
            }
            if (string.IsNullOrEmpty(student.FirstName))
            {
                return BadRequest("First Name is required.");
            }
            if (string.IsNullOrEmpty(student.LastName))
            {
                return BadRequest("Last Name is required.");
            }
            if (string.IsNullOrEmpty(student.Gender))
            {
                return BadRequest("Gender is required.");
            }
            if (student.BirthDay == null)
            {
                return BadRequest("Birthday is required.");
            }
            if (string.IsNullOrEmpty(student.StudentId))
            {
                return BadRequest("Student  ID is required.");
            }
            if (student.Classes != null && student.Classes.Any() && !_dataRepository.AreValidClasses(student.Classes.ToList()))
            {
                return BadRequest("Invalid Classes");
            }

            var studentToUpdate = _dataRepository.Get(id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _dataRepository.Update(studentToUpdate, student);
            return NoContent();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_dataRepository.Exists(id))
            {
                return NotFound("Student not found.");
            }
            var student = _dataRepository.Get(id);
            if (student == null)
            {
                return NotFound("The Student record couldn't be found.");
            }

            _dataRepository.Delete(student);
            return NoContent();
        }
    }
}