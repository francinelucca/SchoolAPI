using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAPI.Models.DTO
{
	public class ClassReceivableDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Classroom { get; set; }
        public int TeacherId { get; set; }
        public ICollection<int> Students { get; set; }
    }
}
