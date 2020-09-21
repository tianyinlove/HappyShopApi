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
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\DB");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            optionsBuilder.UseSqlite($"Data Source=\"{Path.Combine(path, "HappyShop.db")}\"", sqliteOptionsBuilder =>
            {
                sqliteOptionsBuilder.MigrationsAssembly("HappyShop.SQLData");
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
