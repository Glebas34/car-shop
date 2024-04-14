using Microsoft.EntityFrameworkCore;
using CardService.Entities;

namespace CardService.Database
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options){

        }

        public DbSet<Card> Cards {get;set;}
        public DbSet<CarPage> CarPages {get;set;}
    }
}