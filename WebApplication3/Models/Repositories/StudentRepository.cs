using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication3.Models.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _context;

        public StudentRepository(StudentContext context)
        {
            _context = context;
        }

        public IList<Student> GetAll()
        {
            return _context.Students.OrderBy(x => x.StudentName)
                                    .Include(x => x.School).ToList();
        }

        public Student GetById(int id)
        {
            return _context.Students.Where(x => x.StudentId == id)
                                    .Include(x => x.School)
                                    .SingleOrDefault();
        }

        public void Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void Edit(Student student)
        {
            Student existingStudent = _context.Students.Find(student.StudentId);
            if (existingStudent != null)
            {
                existingStudent.StudentName = student.StudentName;
                existingStudent.Age = student.Age;
                existingStudent.BirthDate = student.BirthDate;
                existingStudent.SchoolID = student.SchoolID;
                _context.SaveChanges();
            }
        }

        public void Delete(Student student)
        {
            Student existingStudent = _context.Students.Find(student.StudentId);
            if (existingStudent != null)
            {
                _context.Students.Remove(existingStudent);
                _context.SaveChanges();
            }
        }

        public IList<Student> GetStudentsBySchoolID(int? schoolId)
        {
            return _context.Students.Where(s => s.SchoolID.Equals(schoolId))
                                    .OrderBy(s => s.StudentName)
                                    .Include(std => std.School).ToList();
        }

        public IList<Student> FindByName(string name)
        {
            return _context.Students.Where(s => s.StudentName.Contains(name))
                                    .Include(std => std.School).ToList();
        }
    }
}
