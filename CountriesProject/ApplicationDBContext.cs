namespace CountriesProject
{
    using CountriesProject.Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }        
        public DbSet<CountryDBO> CountriesDBO { get; set; }
  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<CountryDBO>().HasNoKey();
            
        }
    }

}
