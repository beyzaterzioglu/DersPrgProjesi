using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using DersPrgProjesi.Models; // Admin sınıfınızın bulunduğu klasör
namespace DersPrgProjesi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-CB1H4N6\\SQLEXPRESS;Initial Catalog=DersProgramiSitesi;Integrated Security=True;Trust Server Certificate=True", option => {
        //        option.EnableRetryOnFailure();
        //    });
        //    base.OnConfiguring(optionsBuilder);
        //}

       
        public DbSet<admin> Admins { get; set; }
        public DbSet<Fakulte> Fakulteler { get; set; }
    }
}
