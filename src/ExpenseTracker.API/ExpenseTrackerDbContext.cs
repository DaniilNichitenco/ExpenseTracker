using ExpenseTracker.Domain;
using ExpenseTracker.Domain.Auth;
using ExpenseTracker.Domain.Purses;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.EFMapping.Schemas;
using ExpenseTracker.EFMapping;

namespace ExpenseTracker.API
{
    public class ExpenseTrackerDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ExpenseTrackerDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Person> People { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Purse> Purses { get; set; }
        public DbSet<Occasion> Occasions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var assemblyNote = typeof(NoteConfig).Assembly;
            var assemblyOccasion = typeof(OccasionConfig).Assembly;
            var assemblyProfile = typeof(PersonConfig).Assembly;
            var assemblyPurse = typeof(PurseConfig).Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(assemblyNote);
            modelBuilder.ApplyConfigurationsFromAssembly(assemblyOccasion);
            modelBuilder.ApplyConfigurationsFromAssembly(assemblyProfile);
            modelBuilder.ApplyConfigurationsFromAssembly(assemblyPurse);

            modelBuilder.Entity<PurseEUR>();
            modelBuilder.Entity<PurseMDL>();
            modelBuilder.Entity<PurseUSD>();

            ApplyIdentityMapConfiguration(modelBuilder);
        }

        private void ApplyIdentityMapConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users", SchemaConsts.Auth);
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims", SchemaConsts.Auth);
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins", SchemaConsts.Auth);
            modelBuilder.Entity<UserToken>().ToTable("UserRoles", SchemaConsts.Auth);
            modelBuilder.Entity<Role>().ToTable("Roles", SchemaConsts.Auth);
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims", SchemaConsts.Auth);
            modelBuilder.Entity<UserRole>().ToTable("UserRole", SchemaConsts.Auth);
        }
    }
}
