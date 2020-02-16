using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolAPI.Models.DTO;
using SchoolAPI.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAPI.Models.DataManager
{
	public class ClassDataManager : IDataRepository<Class, ClassDTO, ClassReceivableDTO>
	{
		readonly SchoolContext _schoolContext;
		public ClassDataManager(SchoolContext schoolContext)
		{
			_schoolContext = schoolContext;

		}
		public void Add(ClassReceivableDTO entity)
		{
			Class classObj = ClassDTOMapper.MapToDto(entity);
			classObj.CreatedAt = DateTime.Now;
			_schoolContext.Class.Add(classObj);
			_schoolContext.SaveChanges();
			if(entity.Students != null && entity.Students.Any())
			{
				var classStudents = new List<ClassEnrollment>();
				foreach (var student in entity.Students)
				{
					classStudents.Add(new ClassEnrollment
					{
						ClassId = classObj.Id,
						StudentId = student,
						CreatedAt = DateTime.Now,
					});
				}
				_schoolContext.ClassEnrollment.AddRange(classStudents);
				_schoolContext.SaveChanges();
			}
		}

		public void Delete(Class entity)
		{
			_schoolContext.ClassEnrollment.RemoveRange(_schoolContext.ClassEnrollment.Where(c => c.ClassId == entity.Id));
			_schoolContext.Class.Remove(entity);
			_schoolContext.SaveChanges();
		}

		public bool Exists(int id)
		{
			return _schoolContext.Class.Any(a => a.Id == id);
		}

		public bool AreValidStudents(List<int> classes)
		{
			return !classes.Any(s => !_schoolContext.Student.Select(st => st.Id).Contains(s));
		}

		public Class Get(int id)
		{
			var classobj = _schoolContext.Class.SingleOrDefault(s => s.Id == id);

			return classobj;
		}

		public IQueryable<Class> GetAll()
		{
			return _schoolContext.Class;
		}

		public IEnumerable<ClassDTO> GetAllDtos()
		{
			_schoolContext.ChangeTracker.LazyLoadingEnabled = true;

			return this.GetAll().Include(c => c.Teacher).Select(c => ClassDTOMapper.MapToDto(c, false));
		}

		public ClassDTO GetDto(int id)
		{
			_schoolContext.ChangeTracker.LazyLoadingEnabled = true;
			var classObj = _schoolContext.Class
				.Include(c => c.Teacher)
				.Include(c => c.ClassEnrollment)
				.ThenInclude(c => c.Student)
				.SingleOrDefault(c => c.Id == id);

			return ClassDTOMapper.MapToDto(classObj);
		}

		public void Update(Class entityToUpdate, ClassReceivableDTO entity)
		{
			entityToUpdate = _schoolContext.Class
							.Include(s => s.ClassEnrollment)
							.Single(s => s.Id == entityToUpdate.Id);

			entityToUpdate.Name = entity.Name;
			entityToUpdate.Classroom = entity.Classroom;
			entityToUpdate.TeacherId = entity.TeacherId;
			entityToUpdate.UpdatedAt = DateTime.Now;

			if(entity.Students != null && entity.Students.Any())
			{
				var deletedStudents = entityToUpdate.ClassEnrollment.Where(c => !entity.Students.Contains(c.StudentId)).ToList();
				var addedStudents = entity.Students.Where(s => !entityToUpdate.ClassEnrollment.Select(c => c.StudentId).Contains(s));

				deletedStudents.ForEach(classToDelete =>
					entityToUpdate.ClassEnrollment.Remove(
						entityToUpdate.ClassEnrollment.First(c => c.ClassId == classToDelete.ClassId)
						));

				var classStudents = new List<ClassEnrollment>();
				foreach (var addedStudent in addedStudents)
				{
					classStudents.Add(new ClassEnrollment
					{
						ClassId = entityToUpdate.Id,
						StudentId = addedStudent,
						CreatedAt = DateTime.Now,
					});
				}
				_schoolContext.ClassEnrollment.AddRange(classStudents);
			}
			else
			{
				_schoolContext.RemoveRange(entityToUpdate.ClassEnrollment);
			}
			_schoolContext.SaveChanges();
		}
	}
}
