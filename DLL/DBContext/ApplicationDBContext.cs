using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DLL.Model;
using DLL.Model.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DLL.DBContext
{
    public class ApplicationDBContext : IdentityDbContext<
        AppUser,AppRole,int,
        IdentityUserClaim<int>,AppUserRole,IdentityUserLogin<int>,
        IdentityRoleClaim<int>,IdentityUserToken<int>>
    
   
    {
        private const string IsDeleteProperty = "IsDelete";

        private static readonly MethodInfo _propertyMethod = typeof(EF)
            .GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public)?.MakeGenericMethod(typeof(bool));
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
            
        }

        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var parm = Expression.Parameter(type, "it");
            var prop = Expression.Call(_propertyMethod,parm,Expression.Constant(IsDeleteProperty));
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parm);
            return lambda;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerBalance>()
                .Property(p => p.RowVersion).IsConcurrencyToken();
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entity.ClrType) == true)
                {
                    entity.AddProperty(IsDeleteProperty, typeof(bool));
                    modelBuilder.Entity(entity.ClrType).HasQueryFilter(GetIsDeletedRestriction(entity.ClrType));

                }

                modelBuilder.Entity<CourseStudent>()
                    .HasKey(bc => new {bc.CourseId, bc.StudentId});

                modelBuilder.Entity<CourseStudent>()
                    .HasOne(bc => bc.Course)
                    .WithMany(b => b.CourseStudents)
                    .HasForeignKey(bc => bc.CourseId);


                modelBuilder.Entity<CourseStudent>()
                    .HasOne(bc => bc.Student)
                    .WithMany(b => b.CourseStudents)
                    .HasForeignKey(bc => bc.StudentId);
            }
            
            modelBuilder.Entity<AppUser>(b =>
            {
                b.HasMany(e => e.AppUserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

            });

            modelBuilder.Entity<AppRole>(b =>
            {
                b.HasMany(e => e.AppUserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

            });
            modelBuilder.Entity<Course>(c =>
            {
                c.Property(d => d.Name).HasMaxLength(100);
                c.Property(d => d.Code).HasMaxLength(50);
                c.Property(d => d.CreatedBy).HasMaxLength(100);
                c.Property(d => d.LastUpdatedBy).HasMaxLength(100);
                c.Property(d => d.Credit).HasMaxLength(1);
                c.Property(d => d.ImageUrl).HasMaxLength(100);
            });
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSavingDate();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void OnBeforeSavingDate()
        {
            ChangeTracker.DetectChanges();
            var entries = ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached && e.State != EntityState.Unchanged);
            foreach (var entry in entries)
            {
                if (entry.Entity is ITrackable trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            trackable.CreatedAt = DateTimeOffset.Now;
                            trackable.LastUpdatedAt = DateTimeOffset.Now;
                            break;
                        case EntityState.Modified:
                            trackable.LastUpdatedAt = DateTimeOffset.Now;
                            break;
                        case EntityState.Deleted :
                            entry.Property(IsDeleteProperty).CurrentValue = true;
                            entry.State = EntityState.Modified;
                            break;
                    
                    }
                    
                    
                }
                
                
            }
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSavingDate();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public DbSet<Department> Departments{ get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }
        
        
        //for Concurrency Example
        public DbSet<CustomerBalance> CustomerBalances { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        
        //end concurrency example
    }
}