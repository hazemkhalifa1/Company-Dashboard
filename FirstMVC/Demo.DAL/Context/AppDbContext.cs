using Demo.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Context
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Department> Departments { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Department>().HasMany<Employee>(d => d.Employees)
				.WithOne(e => e.Department).HasForeignKey(e => e.DepartmentId).IsRequired(false);
			base.OnModelCreating(modelBuilder);
		}
	}
}
