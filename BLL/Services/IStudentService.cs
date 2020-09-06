using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Model;
using DLL.Repositories;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface IStudentService
    {
        
        Task<Student> InsertAsync(StudentInsertRequestViewModel studentRequest);
       IQueryable<Student> GetAllAsync();
        Task<Student> DeleteAsync(string email);
        Task<Student> GetAAsync(string email);
        Task<Student> UpdateAsync(string email, Student student);
        Task<bool> EmailExists(string email);
        Task<bool> IsIdExists(int id);


    }
    public class StudentService : IStudentService
    
    {
        private readonly IUnitOfWork _unitOfWork;


        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Student> InsertAsync(StudentInsertRequestViewModel studentRequest)
        
        {
            var student = new Student()
            {
                Email =  studentRequest.Email,
                Name =  studentRequest.Name,
                DepartmentId = studentRequest.DepartmentId
            };
            await _unitOfWork.StudentRepository.CreateAsync(student);
            if (await _unitOfWork.SaveCompletedAsync())
            {
                return student;

            }
            throw new AplicationValidationException("student insert has some problem");

        
        }

        public   IQueryable<Student> GetAllAsync()
        {
            return  _unitOfWork.StudentRepository.QueryAll();
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

        public async Task<bool> EmailExists(string email)
        {
            var student = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.Email == email);
            if (student == null)
            {
                return true;

            }

            return false;
        }
        public async Task<bool> IsIdExists(int id)
        {
            var student = await _unitOfWork.StudentRepository.FindSingleAsync(x => x.StudentId == id);
            if (student == null)
            {
                return true;

            }

            return false;
        }
    }
}