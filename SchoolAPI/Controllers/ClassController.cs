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
    [Route("api/Classes")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly ClassDataManager _dataRepository;

        public ClassController(IDataRepository<Class, ClassDTO, ClassReceivableDTO> dataRepository)
        {
            _dataRepository = (ClassDataManager) dataRepository;
        }


        // GET: api/Classes
        [HttpGet]
        public IActionResult Get()
        {
            var classes = _dataRepository.GetAllDtos();
            return Ok(classes);
        }

        // GET: api/Classes/5
        [HttpGet("{id}", Name = "GetClass")]
        public IActionResult Get(int id)
        {
            if (!_dataRepository.Exists(id))
            {
                return NotFound("Class not found.");
            }
            var classObj = _dataRepository.GetDto(id);

            return Ok(classObj);
        }

        // POST: api/Classes
        [HttpPost]
        public IActionResult Post([FromBody] ClassReceivableDTO classobj)
        {
            if (classobj is null)
            {
                return BadRequest("Class is null.");
            }
            if (string.IsNullOrEmpty(classobj.Name))
            {
                return BadRequest("Name is Required");
            }
            if (classobj.Students != null && classobj.Students.Any() && !_dataRepository.AreValidStudents(classobj.Students.ToList()))
            {
                return BadRequest("Invalid Students");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _dataRepository.Add(classobj);
            return CreatedAtRoute("GetClass", new { Id = classobj.Id }, null);
        }

        // PUT: api/Classes/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ClassReceivableDTO classObj)
        {
            if (classObj == null)
            {
                return BadRequest("Class is null.");
            }
            if (string.IsNullOrEmpty(classObj.Name))
            {
                return BadRequest("Name is Required");
            }
            if (!_dataRepository.Exists(id))
            {
                return NotFound("Class not found.");
            }
            if(classObj.Students != null && classObj.Students.Any() && !_dataRepository.AreValidStudents(classObj.Students.ToList()))
            {
                return BadRequest("Invalid Students");
            }

            var classToUpdate = _dataRepository.Get(id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _dataRepository.Update(classToUpdate, classObj);
            return NoContent();
        }

        // DELETE: api/Classes/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_dataRepository.Exists(id))
            {
                return NotFound("Class not found.");
            }
            var classObj = _dataRepository.Get(id);

            _dataRepository.Delete(classObj);
            return NoContent();
        }
    }
}