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

            Employees.ExecuteDelete();
            Roles.ExecuteDelete();
            Projects.ExecuteDelete();
            

            SaveChanges();
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


         protected override void OnModelCreating(ModelBuilder modelBuilder){


            // MemberDTO

            modelBuilder.Entity<EmployeeDTO>()
                .HasMany<RoleDTO>(e => e.Roles)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<EmployeeDTO>()
                .HasMany<SkillDTO>(e => e.Skills)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            
            // RoleDTO

            modelBuilder.Entity<RoleDTO>()
                .HasKey(r => new { r.ProjectId, r.EmployeeId, r.RoleId });

            modelBuilder.Entity<RoleDTO>()
                .HasOne<EmployeeDTO>()
                .WithMany()
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RoleDTO>()
                .HasOne<ProjectDTO>()
                .WithMany()
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RoleDTO>()
                .HasMany<SkillDTO>(r => r.Skills)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            //ProjectDTO
            modelBuilder.Entity<ProjectDTO>()
                .HasMany<RoleDTO>(p => p.Roles)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);



          
        }
    }
}



