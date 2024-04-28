using Microsoft.EntityFrameworkCore;
using ImageService.Entities;

namespace ImageService.Database
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Image> Images { get;set;}
        public DbSet<CarPage> CarPages { get; set;}
    }
}