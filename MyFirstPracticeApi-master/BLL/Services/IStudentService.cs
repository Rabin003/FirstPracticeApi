using System.Collections.Generic;
using System.Threading.Tasks;
using DLL.Model;
using DLL.Repositories;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface IStudentService
    {
        
        Task<Student> InsertAsync(Student student);
        Task<List<Student>> GetAllAsync();
        Task<Student> DeleteAsync(string email);
        Task<Student> GetAAsync(string email);
        Task<Student> UpdateAsync(string email, Student student);
        
    }
    public class StudentService : IStudentService
    
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student> InsertAsync(Student student)
        {
            await _studentRepository.CreateAsync(student);
            if (await _studentRepository.SaveCompletedAsync())
            {
                return student;

            }
            throw new AplicationValidationException("department insert has some problem");

        
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _studentRepository.GetList();
        }

        public async Task<Student> DeleteAsync(string email)
        {
            var dbStudent = await _studentRepository.FindSingleAsync(x => x.Email == email);
            if (dbStudent==null)
            {
                
                throw new AplicationValidationException("student not found");
            }

            _studentRepository.delete(dbStudent);
            if (await _studentRepository.SaveCompletedAsync())
            {
                return dbStudent;

            }
            throw new AplicationValidationException("delete has some issue");
        }
        

        public async Task<Student> GetAAsync(string email)
        {
            return await _studentRepository.FindSingleAsync(x=>x.Email==email);
        }

        public async Task<Student> UpdateAsync(string email, Student student)
        {
            var dbStudent = await _studentRepository.FindSingleAsync(x => x.Email == email);
            if (dbStudent==null)
            {
                
                throw new AplicationValidationException("student not found");
            }

            dbStudent.Name = student.Name;
            _studentRepository.update(dbStudent);
            if (await _studentRepository.SaveCompletedAsync())
            {
                return dbStudent;

            }
            throw new AplicationValidationException("update has some issue");
        }
    }
}