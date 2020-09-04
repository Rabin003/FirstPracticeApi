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
        private readonly IUnitOfWork _unitOfWork;


        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Student> InsertAsync(Student student)
        {
            await _unitOfWork.StudentRepository.CreateAsync(student);
            if (await _unitOfWork.SaveCompletedAsync())
            {
                return student;

            }
            throw new AplicationValidationException("department insert has some problem");

        
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _unitOfWork.StudentRepository.GetList();
        }

        public async Task<Student> DeleteAsync(string email)
        {
            var dbStudent = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.Email == email);
            if (dbStudent==null)
            {
                
                throw new AplicationValidationException("student not found");
            }

            _unitOfWork.StudentRepository.delete(dbStudent);
            if (await _unitOfWork.SaveCompletedAsync())
            {
                return dbStudent;

            }
            throw new AplicationValidationException("delete has some issue");
        }
        

        public async Task<Student> GetAAsync(string email)
        {
            return await _unitOfWork.StudentRepository.FindSingleAsync(x=>x.Email==email);
        }

        public async Task<Student> UpdateAsync(string email, Student student)
        {
            var dbStudent = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.Email == email);
            if (dbStudent==null)
            {
                
                throw new AplicationValidationException("student not found");
            }

            dbStudent.Name = student.Name;
            _unitOfWork.StudentRepository.update(dbStudent);
            if (await _unitOfWork.SaveCompletedAsync())
            {
                return dbStudent;

            }
            throw new AplicationValidationException("update has some issue");
        }
    }
}