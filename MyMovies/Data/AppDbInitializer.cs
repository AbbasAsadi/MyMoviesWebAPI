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

            context?.Directors.AddRange(
                new Director
                {
                    FullName = "Martin Scorsizi",
                },
                new Director
                {
                    FullName = "Cristofer Nowlan"
                },
                new Director
                {
                    FullName = "Hooman Seyedi"
                }
            );
            context?.Movies.AddRange(new Movie
                {
                    Title = "1st Movie Title",
                    Rating = 4.7,
                    Year = 1997,
                },
                new Movie
                {
                    Title = "2st Movie Title",
                    Rating = 7.4,
                    Year = 2014,
                },
                new Movie
                {
                    Title = "3st Movie Title",
                    Rating = 8.6,
                    Year = 2024,
                }, new Movie
                {
                    Title = "4st Movie Title",
                    Rating = 6.4,
                    Year = 2023,
                }, new Movie
                {
                    Title = "5st Movie Title",
                    Rating = 8.4,
                    Year = 2000,
                }
            );


            context.SaveChanges();
        }
    }
}