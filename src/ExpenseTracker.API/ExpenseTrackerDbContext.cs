using ExpenseTracker.Domain;
using ExpenseTracker.Domain.Auth;
using ExpenseTracker.Domain.Wallets;
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
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Topic> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var assemblyUser = typeof(UserConfig).Assembly;
            var assemblyProfile = typeof(UserInfoConfig).Assembly;
            var assemblyWallet = typeof(WalletConfig).Assembly;
            var assemblyTopic = typeof(TopicConfig).Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(assemblyUser);

            modelBuilder.Entity<WalletEUR>();
            modelBuilder.Entity<WalletMDL>();
            modelBuilder.Entity<WalletUSD>();

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
