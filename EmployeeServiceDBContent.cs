namespace EmployeeServiceTest
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EmployeeServiceDBContent : DbContext
    {
        public EmployeeServiceDBContent()
            : base("EmployeeServiceDBContent")
        {
        }

        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .HasMany(e => e.Employee)
                .WithRequired(e => e.Department)
                .HasForeignKey(e => e.Department_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.About)
                .IsUnicode(false);
        }
    }
}
