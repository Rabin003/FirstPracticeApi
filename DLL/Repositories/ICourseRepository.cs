using System.Collections.Generic;
using System.Threading.Tasks;
using DLL.DBContext;
using DLL.Model;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repositories
{
    public interface ICourseRepository : IRepositoryBase<Course>
    {

    }

    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDBContext context) : base(context)
        {

        }
    }
}