using BasketStoreTelegramBot.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketStoreTelegramBot.Models
{
    class DataContext : DbContext, IDisposable
    {
        private static readonly Lazy<DataContext> _dataContext = new Lazy<DataContext>(() => new DataContext());
        public static DataContext Instance { get { return _dataContext.Value; } }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<AdminEntity> Admins { get; set; }
        public DbSet<CatalogProductEntity> CatalogProducts { get; set; }
        public string DbPath { get; }

        public DataContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "BasketStoreTelegramDB.db");

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=C:\Users\vladu\source\repos\BasketStoreTelegramBot\BasketStoreTelegramDB.db");
    }
}
