using DLL.DBContext;
using DLL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DLL
{
    public class DLLDependency
    {
       public static void AllDependency(IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            //repository dependencies

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            // services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            // services.AddTransient<IStudentRepository, StudentRepository>();
        }
    }
}