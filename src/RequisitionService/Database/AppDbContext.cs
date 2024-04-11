using Microsoft.EntityFrameworkCore;
using RequisitionService.Entities;

namespace RequisitionService.Database
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }
        public DbSet<Requisition> Requisitions {get;set;}
        public DbSet<CarPage> CarPages {get;set;}
    }
}