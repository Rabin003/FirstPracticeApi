using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Model;
using DLL.Repositories;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface ICourseService
    {
        Task<Course> InsertAsync(CourseInsertRequestViewModel request);
        Task<List<Course>> GetAllAsync();
        Task<Course> DeleteAsync(string code);
        Task<Course> GetAAsync(string code);
        Task<Course> UpdateAsync(string code, Course aCourse);
        Task<bool> IsCodeExists(string code);
        Task<bool> IsNameExists(string name);
        Task<bool> IsIdExists(int id);
        





    }

    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Course> InsertAsync(CourseInsertRequestViewModel request)
        {
            var course = new Course();
            course.Code = request.Code;
            course.Name = request.Name;
            course.Credit = request.Credit;

            await _unitOfWork.CourseRepository.CreateAsync(course);
            if (await _unitOfWork.SaveCompletedAsync())
            {
                return course;

            }

            throw new AplicationValidationException("department insert has some problem");

        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _unitOfWork.CourseRepository.GetList();
        }

        public async Task<Course> DeleteAsync(string code)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Code == code);
            if (course == null)
            {
                throw new AplicationValidationException("course not found");
            }

            _unitOfWork.CourseRepository.delete(course);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return course;

            }

            throw new Exception("Some problem for delete data");

        }

        public async Task<Course> GetAAsync(string code)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Code == code);
            if (course == null)
            {

                throw new AplicationValidationException("course not found");
            }

            return course;
        }

        public async Task<Course> UpdateAsync(string code, Course aCourse)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Code == code);
            if (course == null)
            {
                throw new AplicationValidationException("course not found");
            }

            if (!string.IsNullOrWhiteSpace(aCourse.Code))
            {
                var existsAlreadyCode = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Code == code);
                if (existsAlreadyCode != null)
                {
                    throw new AplicationValidationException("your updated code already present in our system");

                }


                course.Code = aCourse.Code;

            }

            if (!string.IsNullOrWhiteSpace(aCourse.Name))
            {
                var existsAlreadyCode =
                    await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Name == aCourse.Name);
                if (existsAlreadyCode != null)
                {
                    throw new AplicationValidationException("your updated code already present in our system");

                }

                course.Name = aCourse.Name;
            }

            _unitOfWork.CourseRepository.update(course);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return course;

            }

            throw new AplicationValidationException("in update have some problem");
        }

        public async Task<bool> IsCodeExists(string code)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Code == code);
            if (course == null)
            {
                return true;

            }

            return false;
        }

        public async Task<bool> IsNameExists(string name)
        {
            var course = await _unitOfWork.CourseRepository.FindSingleAsync(x => x.Name == name);
            if (course == null)
            {
                return true;

            }

            return false;
        }
        public async Task<bool> IsIdExists(int id)
        {
            var course= await _unitOfWork.CourseRepository.FindSingleAsync(x => x.CourseId == id);
            if (course == null)
            {
                return true;

            }

            return false;
        }


    }

}