using HeadcountAllocation.DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace HeadcountAllocation.DAL
{

    public class DBcontext : DbContext
    {
        private static DBcontext _instance = null;
        public static string DbPath;

        private static object _lock = new object();

        public virtual DbSet<EmployeeDTO> Employees { get; set; }
        public virtual DbSet<ProjectDTO> Projects { get; set; }
        public virtual DbSet<RoleDTO> Roles { get; set; }
        public virtual DbSet<EmployeeSkillsDTO> EmployeeSkills { get; set; }
        public virtual DbSet<EmployeeLanguagesDTO> EmployeeLanguages { get; set; }
        public virtual DbSet<RoleSkillsDTO> RoleSkills { get; set; }
        public virtual DbSet<RoleLanguagesDTO> RoleLanguages { get; set; }
        public virtual DbSet<TimeZonesDTO> TimeZones { get; set; }
        public virtual DbSet<SkillTypesDTO> SkillTypes { get; set; }
        public virtual DbSet<LanguageTypesDTO> LanguageTypes { get; set; }


        public override void Dispose()
        {
            // Prevent deletion logic when called by EF tooling
            if (AppDomain.CurrentDomain.FriendlyName.Contains("ef"))
                return;
            lock (_lock)
            {
                if (_instance != null)
                {
                    RemoveRangeIfExists(EmployeeSkills);
                    RemoveRangeIfExists(EmployeeLanguages);
                    RemoveRangeIfExists(RoleSkills);
                    RemoveRangeIfExists(RoleLanguages);

                    RemoveRangeIfExists(Employees);
                    RemoveRangeIfExists(Roles);
                    RemoveRangeIfExists(Projects);

                    RemoveRangeIfExists(TimeZones);
                    RemoveRangeIfExists(LanguageTypes);
                    RemoveRangeIfExists(SkillTypes);
                    // Clear child entities first
                
                    // Save changes to apply deletions
                    SaveChanges();

                    // Reset the singleton instance
                    _instance = null;
                }
            }
        }

        private void RemoveRangeIfExists<TEntity>(DbSet<TEntity> dbSet) where TEntity : class
        {
            var entities = dbSet.ToList(); // Ensures EF is tracking them
            if (entities.Any())
            {
                dbSet.RemoveRange(entities);
            }
        }

        public static DBcontext Reset()
        {
            _instance = new DBcontext();
            return _instance;
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
            DbPath = "Server=localhost,1433;Database=HeadCountDB;User Id=sa;Password=YourStrong!Pass123;TrustServerCertificate=True;";
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
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeDTO>()
                .HasMany(e => e.ForeignLanguages)
                .WithOne()
                .HasForeignKey(l => l.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

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
                .HasForeignKey(l => l.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoleDTO>()
                .HasMany(e => e.Skills)
                .WithOne()
                .HasForeignKey(s => s.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProjectDTO Relationships
            modelBuilder.Entity<ProjectDTO>()
                .HasKey(p => p.ProjectId); // Primary Key

            modelBuilder.Entity<ProjectDTO>()
                .HasMany(p => p.Roles)
                .WithOne()
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RoleLanguagesDTO>()
                .HasKey(rl => new { rl.LanguageTypeId, rl.RoleId });

            modelBuilder.Entity<RoleSkillsDTO>()
                .HasKey(rl => new { rl.SkillTypeId, rl.RoleId });

            modelBuilder.Entity<EmployeeLanguagesDTO>()
                .HasKey(rl => new { rl.LanguageTypeId, rl.EmployeeId });

            modelBuilder.Entity<EmployeeSkillsDTO>()
                .HasKey(rl => new { rl.SkillTypeId, rl.EmployeeId });
        }

    }
}



