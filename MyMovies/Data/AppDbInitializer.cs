using MyMovies.Data.Model;

namespace MyMovies.Data;

public class AppDbInitializer
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

            if (context != null && context.Movies.Any()) return;

            var martin = new Director { FullName = "Martin Scorsese" };
            var nolan = new Director { FullName = "Christopher Nolan" };
            var hooman = new Director { FullName = "Hooman Seyedi" };

            context.Movies.AddRange(
                new Movie { Title = "Goodfellas", Rating = 8.7, Year = 1990, Director = martin },
                new Movie { Title = "The Departed", Rating = 8.5, Year = 2006, Director = martin },
                new Movie { Title = "Inception", Rating = 8.8, Year = 2010, Director = nolan },
                new Movie { Title = "Interstellar", Rating = 8.6, Year = 2014, Director = nolan },
                new Movie { Title = "Sheerak", Rating = 7.9, Year = 2023, Director = hooman }
            );

            context.SaveChanges();
        }
    }
}