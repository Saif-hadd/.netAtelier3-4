using System.Collections.Generic;

namespace WebApplication3.Models.Repositories
{
    public interface IStudentRepository
    {
        IList<Student> GetAll();
        Student GetById(int id);
        void Add(Student student);
        void Edit(Student student);
        void Delete(Student student);
        IList<Student> GetStudentsBySchoolID(int? schoolId);
        IList<Student> FindByName(string name);
    }
}
