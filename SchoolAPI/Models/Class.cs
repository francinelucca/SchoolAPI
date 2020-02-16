using System;
using System.Collections.Generic;

namespace SchoolAPI.Models
{
    public partial class Class
    {
        public Class()
        {
            ClassEnrollment = new HashSet<ClassEnrollment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Classroom { get; set; }
        public int TeacherId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<ClassEnrollment> ClassEnrollment { get; set; }
    }
}
