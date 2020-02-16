using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAPI.Models.DTO
{
	public class ClassDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Classroom { get; set; }
        public TeacherDTO Teacher { get; set; }
        public ICollection<StudentDTO> Students { get; set; }
    }
}
