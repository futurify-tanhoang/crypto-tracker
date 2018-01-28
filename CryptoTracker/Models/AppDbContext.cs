using CryptoTracker.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTracker.Models
{
    public class AppDbContext: DbContext
    {
        private readonly IHttpContextAccessor _httpContext;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContext) : base(options)
        {
            _httpContext = httpContext;
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountPermission> AccountsPermissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolesPermissions { get; set; }
        public DbSet<AccountRole> AccountsRoles { get; set; }
        public DbSet<LoginRecord> LoginRecords { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<CryptoCurrency> CryptoCurrencies { get; set; }
        public DbSet<CryptoTransaction> CryptoTransactions { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<CryptoWallet> CryptoWallets { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Skip shadow types
                if (entityType.ClrType == null)
                {
                    continue;
                }

                entityType.Relational().TableName = entityType.ClrType.Name;
            }

            //configure mapping many to many relationship
            modelBuilder.Entity<AccountPermission>().HasKey(ap => new
            {
                ap.AccountId,
                ap.PermissionId
            });

            //configure mapping many to many relationship
            modelBuilder.Entity<AccountRole>().HasKey(ar => new
            {
                ar.AccountId,
                ar.RoleId
            });

            //configure mapping many to many relationship
            modelBuilder.Entity<RolePermission>().HasKey(rp => new
            {
                rp.PermissionId,
                rp.RoleId
            });

            base.OnModelCreating(modelBuilder);
        }

        public static void UpdateDatabase(AppDbContext db)
        {
            db.Database.Migrate();

            db.Seed();
        }

        private void Seed()
        {
            //var a = CreateApplicationRequestFactory.ConvertToXML();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateTimeTracker();

            return base.SaveChangesAsync();
        }

        public override int SaveChanges()
        {
            UpdateTimeTracker();

            return base.SaveChanges();
        }

        /// <summary>
        /// Auto record create and modify time
        /// </summary>
        private void UpdateTimeTracker()
        {
            var currentTime = DateTime.Now;
            int? accountId = null; ;
            if (_httpContext.HttpContext != null && _httpContext.HttpContext.User != null)
            {
                accountId = _httpContext.HttpContext.User.GetAccountId();
            }

            //Find all Entities that are Added/Modified that inherit from my EntityBase
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Metadata.FindProperty("CreatedAt") != null)
                    {
                        if (entry.Property("CreatedAt").CurrentValue == null)
                            entry.Property("CreatedAt").CurrentValue = currentTime;
                    }

                    if (entry.Metadata.FindProperty("CreatedBy") != null)
                    {
                        if (entry.Property("CreatedBy").CurrentValue == null)
                            entry.Property("CreatedBy").CurrentValue = accountId;
                    }
                }

                if (entry.Metadata.FindProperty("ModifiedAt") != null)
                {
                    if (entry.Property("ModifiedAt").CurrentValue == null)
                        entry.Property("ModifiedAt").CurrentValue = currentTime;
                }

                if (entry.Metadata.FindProperty("ModifiedBy") != null)
                {
                    if (entry.Property("ModifiedBy").CurrentValue == null)
                        entry.Property("ModifiedBy").CurrentValue = accountId;
                }
            }
        }
    }
}
