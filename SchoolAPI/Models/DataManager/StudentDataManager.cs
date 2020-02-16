using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models.DTO;
using SchoolAPI.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAPI.Models.DataManager
{
	public class StudentDataManager : IDataRepository<Student, StudentDTO, StudentReceivableDTO>
	{
		readonly SchoolContext _schoolContext;

		public StudentDataManager(SchoolContext schoolContext)
		{
			_schoolContext = schoolContext;
		}
		public void Add(StudentReceivableDTO entity)
		{
			Student student = StudentDTOMapper.MapToDto(entity);
			student.CreatedAt = DateTime.Now;
			_schoolContext.Student.Add(student);
			_schoolContext.SaveChanges();
			if(entity.Classes != null && entity.Classes.Any())
			{
				var studentClasses = new List<ClassEnrollment>();
				foreach (var classes in entity.Classes)
				{
					studentClasses.Add(new ClassEnrollment
					{
						ClassId = classes,
						StudentId = student.Id,
						CreatedAt = DateTime.Now,
					});
				}
				_schoolContext.ClassEnrollment.AddRange(studentClasses);
				_schoolContext.SaveChanges();
			}
		}

		public void Delete(Student entity)
		{
			_schoolContext.ClassEnrollment.RemoveRange(_schoolContext.ClassEnrollment.Where(c => c.StudentId == entity.Id));
			_schoolContext.Student.Remove(entity);
			_schoolContext.SaveChanges();
		}
		public bool Exists(int id)
		{
			return _schoolContext.Student.Any(a => a.Id == id);
		}

		public bool AreValidClasses(List<int> classes)
		{
			return !classes.Any(c => !_schoolContext.Class.Select(cl => cl.Id).Contains(c));
		}

		public Student Get(int id)
		{
			var student = _schoolContext.Student.SingleOrDefault(s => s.Id == id);

			return student;
		}

		public IQueryable<Student> GetAll()
		{
			return _schoolContext.Student;
		}

		public IEnumerable<StudentDTO> GetAllDtos()
		{
			_schoolContext.ChangeTracker.LazyLoadingEnabled = true;
			return this.GetAll().Select(s => StudentDTOMapper.MapToDto(s, false));

		}

		public StudentDTO GetDto(int id)
		{
			_schoolContext.ChangeTracker.LazyLoadingEnabled = true;

			var student = _schoolContext.Student
				.Include(s => s.ClassEnrollment)
				.ThenInclude(c => c.Class)
				.ThenInclude(c => c.Teacher)
				.SingleOrDefault(c => c.Id == id);

			return StudentDTOMapper.MapToDto(student);
		}

		public void Update(Student entityToUpdate, StudentReceivableDTO entity)
		{
			entityToUpdate = _schoolContext.Student
							.Include(s => s.ClassEnrollment)
							.Single(s => s.Id == entityToUpdate.Id);

			entityToUpdate.FirstName = entity.FirstName;
			entityToUpdate.LastName = entity.LastName;
			entityToUpdate.BirthDay = entity.BirthDay;
			entityToUpdate.Gender = entity.Gender;
			entityToUpdate.StudentId = entity.StudentId;
			entityToUpdate.UpdatedAt = DateTime.Now;

			if(entity.Classes != null && entity.Classes.Any())
			{

				var deletedClasses = entityToUpdate.ClassEnrollment.Where(c => !entity.Classes.Contains(c.ClassId)).ToList();
				var addedClasses = entity.Classes.Where(c => !entityToUpdate.ClassEnrollment.Select(cl => cl.ClassId).Contains(c));

				deletedClasses.ForEach(classToDelete =>
					entityToUpdate.ClassEnrollment.Remove(
						entityToUpdate.ClassEnrollment.First(c => c.ClassId == classToDelete.ClassId)
						));

				var studentClasses = new List<ClassEnrollment>();
				foreach (var addedClass in addedClasses)
				{
					studentClasses.Add(new ClassEnrollment
					{
						ClassId = addedClass,
						StudentId = entityToUpdate.Id,
						CreatedAt = DateTime.Now,
					});
				}
				_schoolContext.ClassEnrollment.AddRange(studentClasses);
			}
			else
			{
				_schoolContext.ClassEnrollment.RemoveRange(entityToUpdate.ClassEnrollment);
			}
			_schoolContext.SaveChanges();
		}
	}
}
