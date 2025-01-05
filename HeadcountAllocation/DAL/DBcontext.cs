using HeadcountAllocation.DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace HeadcountAllocation.DAL{

    public class DBcontext : DbContext
    {
        private static DBcontext _instance = null;
        public static string DbPath;

        private static object _lock = new object();

        public virtual DbSet<EmployeeDTO> Employees { get; set; }
        public virtual DbSet<ProjectDTO> Projects { get; set; }
        public virtual DbSet<RoleDTO> Roles { get; set; }


         public override void Dispose()
        {

            // Employees.ExecuteDelete();
            // Roles.ExecuteDelete();
            // Projects.ExecuteDelete();
            

            // SaveChanges();
            _instance = new DBcontext();
        }


        public static DBcontext GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DBcontext();
                    }
                }
            }
            return _instance;
        }

        public DBcontext()
        {
            DbPath = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HeadCountDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;Application Intent=ReadWrite;MultiSubnetFailover=False";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                  optionsBuilder.UseSqlServer($"{DbPath}"); // Use DbPath to configure the database connection
            }
        }


         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // EmployeeDTO Relationships
            modelBuilder.Entity<EmployeeDTO>()
                .HasKey(e => e.EmployeeId); // Primary Key

            modelBuilder.Entity<EmployeeDTO>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmployeeDTO>()
                .HasMany(e => e.Skills)
                .WithOne()
                .HasForeignKey(s => s.EmployeeId);

            modelBuilder.Entity<EmployeeDTO>()
                .HasMany(e => e.ForeignLanguages)
                .WithOne()
                .HasForeignKey(l => l.EmployeeId);

            // RoleDTO Relationships
            modelBuilder.Entity<RoleDTO>()
                .HasKey(r => r.RoleId); // Primary Key

            modelBuilder.Entity<RoleDTO>()
                .HasOne<EmployeeDTO>()
                .WithMany(e => e.Roles) // Explicitly reference navigation property
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RoleDTO>()
                .HasOne<ProjectDTO>()
                .WithMany()
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoleDTO>()
                .HasMany(e => e.ForeignLanguages)
                .WithOne()
                .HasForeignKey(l => l.RoleId);

            modelBuilder.Entity<RoleDTO>()
                .HasMany(e => e.Skills)
                .WithOne()
                .HasForeignKey(s => s.RoleId);

            // ProjectDTO Relationships
            modelBuilder.Entity<ProjectDTO>()
                .HasKey(p => p.ProjectId); // Primary Key

            modelBuilder.Entity<ProjectDTO>()
                .HasMany(p => p.Roles)
                .WithOne()
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}



