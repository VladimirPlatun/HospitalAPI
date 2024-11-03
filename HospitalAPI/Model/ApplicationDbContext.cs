using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<NameInfo> NameInfo { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Name)
                .WithOne(n => n.Patient)
                .HasForeignKey<NameInfo>(n => n.PatientId)
                .OnDelete(DeleteBehavior.Cascade); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=sql_server,1433;Database=HospitalDB;User Id=sa;Password=YourPassword123;TrustServerCertificate=True;MultipleActiveResultSets=true;",
                sqlOptions => sqlOptions.EnableRetryOnFailure());

            
        }
    }
}

