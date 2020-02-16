using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAPI.Models.DTO
{
	public static class TeacherDTOMapper
    {
        public static TeacherDTO MapToDto(Teacher teacher, bool includeClasses = true)
        {
            return new TeacherDTO()
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Gender = teacher.Gender,
                IdCard = teacher.IdCard,

                Classes = includeClasses ? teacher.Class.Select(cl => ClassDTOMapper.MapToDto(cl, false)).ToList() : null
            };
        }
    }
}
