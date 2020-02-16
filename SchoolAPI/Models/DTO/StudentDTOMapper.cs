using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAPI.Models.DTO
{
	public static class StudentDTOMapper
    {
        public static StudentDTO MapToDto(Student student, bool includeClasses = true)
        {
            return new StudentDTO()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Gender = student.Gender,
                BirthDay = student.BirthDay,
                StudentId = student.StudentId,

                Classes = includeClasses ? student.ClassEnrollment.Select(ce => ClassDTOMapper.MapToDto(ce.Class,false)).ToList() : null
            };
        }
        public static Student MapToDto(StudentReceivableDTO student)
        {
            return new Student()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Gender = student.Gender,
                BirthDay = student.BirthDay,
                StudentId = student.StudentId,
            };
        }
    }
}
