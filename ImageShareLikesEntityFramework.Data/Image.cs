using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ImageShareLikesEntityFramework.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public int Likes { get; set; }
        public DateTime Date { get; set; }
    }

    public class ImageDbContext : DbContext
    {
        private readonly string _connectionString;
        public ImageDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        public DbSet<Image> Images { get; set; }
    }

    public class ImageContextFactory : IDesignTimeDbContextFactory<ImageDbContext>
    {
        public ImageDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}ImageShareLikesEntityFramework.Web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new ImageDbContext(config.GetConnectionString("ConStr"));
        }
    }
}
