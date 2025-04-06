using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.DAL.DTO.Alert;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        public virtual DbSet<TicketDTO> Tickets {get; set;}
        public virtual DbSet<EventDTO> Events {get; set;}
        public virtual DbSet<MessageDTO> Messages {get; set;}



        public void ClearDatabase()
        {
            // Delete from most dependent tables first
            EmployeeSkills.ExecuteDelete();      // FK to Employees, SkillTypes
            EmployeeLanguages.ExecuteDelete();   // FK to Employees, LanguageTypes
            RoleSkills.ExecuteDelete();          // FK to Roles, SkillTypes
            RoleLanguages.ExecuteDelete();       // FK to Roles, LanguageTypes

            Roles.ExecuteDelete();               // FK to Projects, TimeZones
            Employees.ExecuteDelete();           // FK to TimeZones
            Projects.ExecuteDelete();            // no FKs to Projects

            SkillTypes.ExecuteDelete();          // referenced by EmployeeSkills / RoleSkills
            LanguageTypes.ExecuteDelete();       // referenced by EmployeeLanguages / RoleLanguages
            TimeZones.ExecuteDelete();           // referenced by Employees / Roles

            SaveChanges();
            _instance = new DBcontext();
        }

        public override void Dispose()
        {
            // Tickets.ExecuteDelete();
            // Messages.ExecuteDelete();
            // Events.ExecuteDelete();
            // EmployeeSkills.ExecuteDelete();      // FK to Employees, SkillTypes
            // EmployeeLanguages.ExecuteDelete();   // FK to Employees, LanguageTypes
            // RoleSkills.ExecuteDelete();          // FK to Roles, SkillTypes
            // RoleLanguages.ExecuteDelete();       // FK to Roles, LanguageTypes

            // Roles.ExecuteDelete();               // FK to Projects, TimeZones
            // Employees.ExecuteDelete();           // FK to TimeZones
            // Projects.ExecuteDelete();            // no FKs to Projects

            // SkillTypes.ExecuteDelete();          // referenced by EmployeeSkills / RoleSkills
            // LanguageTypes.ExecuteDelete();       // referenced by EmployeeLanguages / RoleLanguages
            // TimeZones.ExecuteDelete();           // referenced by Employees / Roles

            // SaveChanges();
            // _instance = new DBcontext();
        }

        private void RemoveRangeIfExists<TEntity>(DbSet<TEntity> dbSet) where TEntity : class
        {
            var entities = dbSet.ToList(); // tracked
            dbSet.RemoveRange(entities);
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

        public DBcontext(DbContextOptions<DBcontext> options) : base(options) { }

        public DBcontext()
        {
            DbPath = "Server=localhost,1433;Database=HeadCountDB;User Id=sa;Password=YourStrong!Pass123;TrustServerCertificate=True;";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .Build();

                var connectionString = config.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                }

                optionsBuilder.UseSqlServer(connectionString);
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

            modelBuilder.Entity<TicketDTO>()
                .HasKey(t => t.TicketId);

            modelBuilder.Entity<TicketDTO>()
                .HasOne<EmployeeDTO>()
                .WithMany()
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EventDTO>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<EventDTO>()
                .HasOne(e => e.Listener)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MessageDTO>()
                .HasKey(m => m.Id);
        }

    }
}



