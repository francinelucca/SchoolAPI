using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAPI.Models.DTO
{
	public class StudentReceivableDTO
    {
        public StudentReceivableDTO()
        {
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Gender { get; set; }
        public string StudentId { get; set; }

        public ICollection<int> Classes { get; set; }
    }
}
