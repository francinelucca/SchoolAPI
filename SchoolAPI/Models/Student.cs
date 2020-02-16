using System;
using System.Collections.Generic;

namespace SchoolAPI.Models
{
    public partial class Student
    {
        public Student()
        {
            ClassEnrollment = new HashSet<ClassEnrollment>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string StudentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<ClassEnrollment> ClassEnrollment { get; set; }
    }
}
