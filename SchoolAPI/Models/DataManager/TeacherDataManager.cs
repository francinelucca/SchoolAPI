using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models.DTO;
using SchoolAPI.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAPI.Models.DataManager 
{
	public class TeacherDataManager : IDataRepository<Teacher, TeacherDTO, Teacher>
	{
		readonly SchoolContext _schoolContext;
		public TeacherDataManager(SchoolContext schoolContext)
		{
			_schoolContext = schoolContext;
		}

		public void Add(Teacher entity)
		{
			entity.CreatedAt = DateTime.Now;
			_schoolContext.Teacher.Add(entity);
			_schoolContext.SaveChanges();
		}
		public void Delete(Teacher entity)
		{
			_schoolContext.Teacher.Remove(entity);
			_schoolContext.SaveChanges();
		}
		public bool Exists(int id)
		{
			return _schoolContext.Teacher.Any(a => a.Id == id);
		}

		public Teacher Get(int id)
		{
			var teacher = _schoolContext.Teacher
				.Include(t => t.Class)
				.SingleOrDefault(s => s.Id == id);

			return teacher;
		}

		public IQueryable<Teacher> GetAll()
		{
			return _schoolContext.Teacher;
		}

		public IEnumerable<TeacherDTO> GetAllDtos()
		{
			return this.GetAll().Select(t => TeacherDTOMapper.MapToDto(t,false));
		}

		public TeacherDTO GetDto(int id)
		{
			_schoolContext.ChangeTracker.LazyLoadingEnabled = true;

			var teacher = _schoolContext.Teacher
				.Include(t => t.Class)
				.SingleOrDefault(c => c.Id == id);

			return TeacherDTOMapper.MapToDto(teacher);
		}

		public void Update(Teacher entityToUpdate, Teacher entity)
		{
			entityToUpdate = _schoolContext.Teacher
							.Single(s => s.Id == entityToUpdate.Id);

			entityToUpdate.FirstName = entity.FirstName;
			entityToUpdate.LastName = entity.LastName;
			entityToUpdate.Gender = entity.Gender;
			entityToUpdate.IdCard = entity.IdCard;
			entityToUpdate.UpdatedAt = DateTime.Now;

			_schoolContext.SaveChanges();
		}
	}
}
