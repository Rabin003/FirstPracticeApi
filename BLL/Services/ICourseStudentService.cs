using System;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Model;
using DLL.Repositories;
using DLL.ResponseViewModel;
using Utility.Exceptions;
using Utility.Models;

namespace BLL.Services
{
    public interface ICourseStudentService
    {
        Task<ApiSuccessResponse> InsetAsync(CourseAssignInsertViewModel request);
        Task<StudentCourseViewModel> CourseListAsync(int studentId);

    }

    public class CourseStudentService : ICourseStudentService
    
    {
        private readonly IUnitOfWork _unitOfWork;

        public  CourseStudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiSuccessResponse> InsetAsync(CourseAssignInsertViewModel request)
        {
            var isStudentAlreadyEnroll =
                await _unitOfWork.CourseStudentRepository.FindSingleAsync(x => 
                    x.CourseId == request.CourseId && x.StudentId==request.StudentId);
            if (isStudentAlreadyEnroll != null)
            {
                throw new AplicationValidationException("this student already enroll in this course" );
                
            }
            var courseStudent = new CourseStudent()
            {
                CourseId = request.CourseId,
                StudentId = request.StudentId
            };

            await _unitOfWork.CourseStudentRepository.CreateAsync(courseStudent);
            if (await _unitOfWork.SaveCompletedAsync())
            {
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "student enroll successfully"
                };
                
            }
            
            throw new AplicationValidationException("something wrong for enrollment" );
        }

        public async Task<StudentCourseViewModel> CourseListAsync(int studentId)
        {
            return await _unitOfWork.StudentRepository.GetSpecificStudentCourseListAsync(studentId);
        }
    }
    
    
}