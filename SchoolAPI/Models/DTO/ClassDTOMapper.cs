using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAPI.Models.DTO
{
	public static class ClassDTOMapper
    {
        public static ClassDTO MapToDto(Class classObj, bool includeStudents = true)
        {
            return new ClassDTO()
            {
                Id = classObj.Id,
                Name = classObj.Name,
                Classroom = classObj.Classroom,
                Teacher = TeacherDTOMapper.MapToDto(classObj.Teacher, false),

                Students = includeStudents ? classObj.ClassEnrollment.Select(ce => StudentDTOMapper.MapToDto(ce.Student, false)).ToList() : null
            };
        }
        public static Class MapToDto(ClassReceivableDTO classObj)
        {
            return new Class()
            {
                Id = classObj.Id,
                Name = classObj.Name,
                Classroom = classObj.Classroom,
                TeacherId = classObj.TeacherId,
            };
        }
    }
}
