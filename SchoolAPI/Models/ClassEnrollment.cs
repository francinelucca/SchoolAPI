using System;
using System.Collections.Generic;

namespace SchoolAPI.Models
{
    public partial class ClassEnrollment
    {
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Class Class { get; set; }
        public virtual Student Student { get; set; }
    }
}
