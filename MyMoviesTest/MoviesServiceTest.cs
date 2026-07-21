using Microsoft.EntityFrameworkCore;
using MyMovies.Data;
using MyMovies.Data.Model;
using MyMovies.Data.Service;
using MyMovies.Data.ViewModels;
using MyMovies.Exception;

namespace MyMoviesTest;

public class MoviesServiceTest
{
    private static readonly DbContextOptions<AppDbContext> DbContextOptions =
        new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("MovieDbControllerTest")
            .Options;

    private AppDbContext _context;
    private MoviesService _moviesService;

    [OneTimeSetUp]
    public void Setup()
    {
        _context = new AppDbContext(DbContextOptions);
        _context.Database.EnsureCreated();

        SeedDatabase();

        _moviesService =
            new MoviesService(_context);
    }

    [Test]
    public void HttpGet_GetAllMovies_WithNoSort_WithNoFilter_WithNoPagination_ReturnOk_Test()
    {
        var result = _moviesService.GetAllMovieV1(
            null,
            null,
            null,
            null,
            null,
            null
        );
        Assert.That(result.Count, Is.EqualTo(5));
    }

    [Test]
    public void HttpGet_GetMovieById_ReturnOk_Test()
    {
        var result = _moviesService.GetMovieById(5);
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void HttpGet_GetMovieById_ReturnNotOk_Test()
    {
        Assert.Throws<NotFoundException>(() => _moviesService.GetMovieById(200));
    }

    [Test]
    public void HttpPost_AddMovie_ReturnOk_Test()
    {
        var result = _moviesService.AddMovie(new MovieVM
        {
            Title = "Tenet",
            Rating = 7.6,
            Year = 2022,
            DirectorId = 2
        });
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void HttpDelete_DeleteMovie_ReturnOk_Test()
    {
        _moviesService.DeleteMovie(2);
        Assert.Throws<NotFoundException>(() => _moviesService.GetMovieById(2));
    }

    [Test]
    public void HttpDelete_DeleteMovie_ReturnNotOk_Test()
    {
        Assert.Throws<NotFoundException>(() => _moviesService.DeleteMovie(200));
    }

    private void SeedDatabase()
    {
        var tarantino = new Director { FullName = "Quentin Tarantino" };
        var spielberg = new Director { FullName = "Steven Spielberg" };
        var fincher = new Director { FullName = "David Fincher" };
        var villeneuve = new Director { FullName = "Denis Villeneuve" };
        var cameron = new Director { FullName = "James Cameron" };
        var kubrick = new Director { FullName = "Stanley Kubrick" };
        var scott = new Director { FullName = "Ridley Scott" };
        var bong = new Director { FullName = "Bong Joon-ho" };
        var darabont = new Director { FullName = "Frank Darabont" };
        var zemeckis = new Director { FullName = "Robert Zemeckis" };

        _context.Movies.AddRange(
            new Movie { Title = "Pulp Fiction", Rating = 8.9, Year = 1994, Director = tarantino },
            new Movie { Title = "Django Unchained", Rating = 8.5, Year = 2012, Director = tarantino },
            new Movie { Title = "Kill Bill: Vol. 1", Rating = 8.2, Year = 2003, Director = tarantino },
            new Movie { Title = "Jurassic Park", Rating = 8.2, Year = 1993, Director = spielberg },
            new Movie { Title = "Saving Private Ryan", Rating = 8.6, Year = 1998, Director = spielberg },
            new Movie { Title = "Schindler's List", Rating = 9.0, Year = 1993, Director = spielberg },
            new Movie { Title = "Fight Club", Rating = 8.8, Year = 1999, Director = fincher },
            new Movie { Title = "Se7en", Rating = 8.6, Year = 1995, Director = fincher },
            new Movie { Title = "Gone Girl", Rating = 8.1, Year = 2014, Director = fincher },
            new Movie { Title = "Arrival", Rating = 7.9, Year = 2016, Director = villeneuve },
            new Movie { Title = "Blade Runner 2049", Rating = 8.0, Year = 2017, Director = villeneuve },
            new Movie { Title = "Dune", Rating = 8.0, Year = 2021, Director = villeneuve },
            new Movie { Title = "Titanic", Rating = 7.9, Year = 1997, Director = cameron },
            new Movie { Title = "Avatar", Rating = 7.8, Year = 2009, Director = cameron },
            new Movie { Title = "The Terminator", Rating = 8.1, Year = 1984, Director = cameron },
            new Movie { Title = "The Shining", Rating = 8.4, Year = 1980, Director = kubrick },
            new Movie { Title = "A Clockwork Orange", Rating = 8.3, Year = 1971, Director = kubrick },
            new Movie { Title = "Gladiator", Rating = 8.5, Year = 2000, Director = scott },
            new Movie { Title = "Alien", Rating = 8.5, Year = 1979, Director = scott },
            new Movie { Title = "Parasite", Rating = 8.5, Year = 2019, Director = bong },
            new Movie { Title = "Memories of Murder", Rating = 8.1, Year = 2003, Director = bong },
            new Movie { Title = "The Shawshank Redemption", Rating = 9.3, Year = 1994, Director = darabont },
            new Movie { Title = "The Green Mile", Rating = 8.6, Year = 1999, Director = darabont },
            new Movie { Title = "Forrest Gump", Rating = 8.8, Year = 1994, Director = zemeckis },
            new Movie { Title = "Cast Away", Rating = 7.8, Year = 2000, Director = zemeckis }
        );
        _context.SaveChanges();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}