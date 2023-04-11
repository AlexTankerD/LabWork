using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp8.Model;
namespace WpfApp8
{
    public class AppContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public AppContext() => Database.EnsureCreated();
        string folderpath = Path.Combine(Directory.GetCurrentDirectory(), @"DB\Products.db");

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source = {folderpath}");
        }
    }

}
