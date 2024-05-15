using Microsoft.EntityFrameworkCore;
using ReservaDeSalas.Model;

namespace ReservaDeSalas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Reserva> Reservas { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite(connectionString: "DataSource=app.db; Cache=Shared");
        //}
    }
}
