using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CourseEnrollController : MainApiController
    {
        private readonly ICourseStudentService _courseStudentService;

        public CourseEnrollController(ICourseStudentService courseStudentService)
        {
            _courseStudentService = courseStudentService;
        }
        [HttpPost]
        public async Task<IActionResult> Insert(CourseAssignInsertViewModel request)
        {
            return Ok(await _courseStudentService.InsetAsync(request));
        }
        
        [HttpGet("{studentId}")]
        public async Task<IActionResult> CourseList(int studentId)
        {
            return Ok(await _courseStudentService.CourseListAsync(studentId));
        }
    }
}