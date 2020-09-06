using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class CourseAssignInsertViewModel
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
    public class CourseAssignInsertViewModelValidator : AbstractValidator<CourseAssignInsertViewModel>
    {
        private readonly IServiceProvider _serviceProvider;

        public CourseAssignInsertViewModelValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleFor(x => x.CourseId).NotNull().NotEmpty().MustAsync(CourseIdExists)
                .WithMessage("Course not exist in our system");
            RuleFor(x => x.StudentId).NotNull().NotEmpty().MustAsync(StudentIdExists)
                .WithMessage("student id not exist in our system");
        }

        private async Task<bool> CourseIdExists(int courseId, CancellationToken arg2)
        {
            

            var requiredService = _serviceProvider.GetRequiredService<ICourseService>();
            return ! await requiredService.IsIdExists(courseId);

        }

        private async Task<bool> StudentIdExists(int studentId, CancellationToken arg2)
        {
            
            var requiredService = _serviceProvider.GetRequiredService<IStudentService>();
            return ! await requiredService.IsIdExists(studentId);

        }

    }

}