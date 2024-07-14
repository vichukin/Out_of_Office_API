using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Data;

namespace Out_of_Office_API.Data
{
    public class CompanyDBContext : IdentityDbContext<Employee>
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public CompanyDBContext(DbContextOptions<CompanyDBContext> options) : base(options)
        {
            Database.EnsureCreated();

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка отношений для Employee и его PeoplePartner
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.PeoplePartner)
                .WithMany()
                .HasForeignKey(e => e.PeoplePartnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка отношений для проектов, которые работник ведет как менеджер
            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectManager)
                .WithMany(e => e.OwnedProjects)
                .HasForeignKey(p => p.ProjectManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка отношений для проектов, в которых работник участвует
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Members)
                .WithMany(e => e.Projects)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectMember",
                    j => j
                        .HasOne<Employee>()
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .HasConstraintName("FK_ProjectMember_Employee_EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Project>()
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .HasConstraintName("FK_ProjectMember_Project_ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                );
        }


    }
}
