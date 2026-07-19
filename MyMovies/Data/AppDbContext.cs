using Microsoft.EntityFrameworkCore;
using MyMovies.Data.Model;

namespace MyMovies.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Director> Directors { get; set; }

}