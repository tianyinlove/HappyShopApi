using HappyShop.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HappyShop.Data
{
    /// <summary>
    /// 
    /// </summary>
    internal class HappyShopSQLContext : DbContext
    {
        static string _dbConnectionString;

        /// <summary>
        /// 
        /// </summary>
        public HappyShopSQLContext()
        {
            if (string.IsNullOrEmpty(_dbConnectionString))
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\DB");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                _dbConnectionString = Path.Combine(filePath, "HappyShop.db");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<UserInfoEntity> UserInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void EnsureMigrate()
        {
            Database.Migrate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=\"{_dbConnectionString}\"", sqliteOptionsBuilder =>
            {
                sqliteOptionsBuilder.MigrationsAssembly("HappyShop.Data");
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfoEntity>(entity =>
            {
                entity.ToTable("UserInfo");
                entity.HasKey(x => x.Id);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
