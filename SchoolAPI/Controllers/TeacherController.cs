using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models;
using SchoolAPI.Models.DTO;
using SchoolAPI.Models.Repository;

namespace SchoolAPI.Controllers
{
    [Route("api/Teachers")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IDataRepository<Teacher, TeacherDTO, Teacher> _dataRepository;

        public TeacherController(IDataRepository<Teacher, TeacherDTO, Teacher> dataRepository)
        {
            _dataRepository = dataRepository;
        }


        // GET: api/Teachers
        [HttpGet]
        public IActionResult Get()
        {
            var teachers = _dataRepository.GetAllDtos();
            return Ok(teachers);
        }

        // GET: api/Teachers/5
        [HttpGet("{id}", Name = "GetTeacher")]
        public IActionResult Get(int id)
        {
            if (!_dataRepository.Exists(id))
            {
                return NotFound("Teacher not found.");
            }
            var teacher = _dataRepository.GetDto(id);

            return Ok(teacher);
        }

        // POST: api/Teachers
        [HttpPost]
        public IActionResult Post([FromBody] Teacher teacher)
        {
            if (teacher is null)
            {
                return BadRequest("Teacher is null.");
            }
            if (string.IsNullOrEmpty(teacher.FirstName))
            {
                return BadRequest("First Name is required.");
            }
            if (string.IsNullOrEmpty(teacher.LastName))
            {
                return BadRequest("Last Name is required.");
            }
            if (string.IsNullOrEmpty(teacher.Gender))
            {
                return BadRequest("Gender is required.");
            }
            if (string.IsNullOrEmpty(teacher.IdCard))
            {
                return BadRequest("Identification Card is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _dataRepository.Add(teacher);
            return CreatedAtRoute("GetTeacher", new { Id = teacher.Id }, null);
        }

        // PUT: api/Teachers/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Teacher teacher)
        {
            if (!_dataRepository.Exists(id))
            {
                return NotFound("Teacher not found.");
            }
            if (teacher == null)
            {
                return BadRequest("Teacher is null.");
            }
            if (string.IsNullOrEmpty(teacher.FirstName))
            {
                return BadRequest("First Name is required.");
            }
            if (string.IsNullOrEmpty(teacher.LastName))
            {
                return BadRequest("Last Name is required.");
            }
            if (string.IsNullOrEmpty(teacher.Gender))
            {
                return BadRequest("Gender is required.");
            }
            if (string.IsNullOrEmpty(teacher.IdCard))
            {
                return BadRequest("Identification Card is required.");
            }

            var teacherToUpdate = _dataRepository.Get(id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _dataRepository.Update(teacherToUpdate, teacher);
            return NoContent();
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_dataRepository.Exists(id))
            {
                return NotFound("Student not found.");
            }
            var teacher = _dataRepository.Get(id);
            if (teacher.Class.Any())
            {
                return BadRequest("The Teacher is assigned to several classes and can't be deleted at the moment");
            }

            _dataRepository.Delete(teacher);
            return NoContent();
        }
    }
}