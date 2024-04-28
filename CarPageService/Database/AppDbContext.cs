using CarPageService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarPageService.Database
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        
        public DbSet<CarPage> CarPages {get;set;}
        public DbSet<Image> Images {get;set;}
    }
}