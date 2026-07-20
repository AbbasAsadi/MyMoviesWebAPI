using Microsoft.EntityFrameworkCore;
using MyMovies.Data.Model;
using MyMovies.Data.ViewModels;

namespace MyMovies.Data.Service;

public class MoviesService(AppDbContext _context)
{
    public MovieResponseVM AddMovie(MovieVM movie)
    {
        var newMovie = new Movie
        {
            Title = movie.Title,
            Year = movie.Year,
            Rating = movie.Rating,
            DirectorId = movie.DirectorId,
        };
        _context.Movies.Add(newMovie);
        _context.SaveChanges();

        _context.Entry(newMovie).Reference(m => m.Director).Load();

        return new MovieResponseVM
        {
            Id = newMovie.Id,
            Title = newMovie.Title,
            Year = newMovie.Year,
            Rating = newMovie.Rating,
            Director = new DirectorVM
            {
                Id = newMovie.Director.Id,
                FullName = newMovie.Director.FullName
            }
        };
    }

    public List<MovieResponseVM> GetAllMovie()
    {
        return _context.Movies.Include(movie => movie.Director).Select(movie =>
            new MovieResponseVM
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Rating = movie.Rating,
                Director = new DirectorVM
                {
                    Id = movie.Director.Id,
                    FullName = movie.Director.FullName
                }
            }).ToList();
    }

    public MovieResponseVM GetMovieById(int movieId)
    {
        var movie = _context.Movies.Include(movie => movie.Director).FirstOrDefault(m => m.Id == movieId);
        return movie == null
            ? throw new Exception($"Movie with id {movieId} not found")
            : new MovieResponseVM
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Rating = movie.Rating,
                Director = new DirectorVM
                {
                    Id = movie.Director.Id,
                    FullName = movie.Director.FullName
                }
            };
    }

    public void DeleteMovie(int movieId)
    {
        var movie = _getMovieById(movieId);
        if (movie == null) throw new Exception($"Movie with id {movieId} not found");
        _context.Movies.Remove(movie);
        _context.SaveChanges();
    }


    public MovieResponseVM UpdateMovie(int movieId, MovieVM updatedMovie)
    {
        var movie = _getMovieById(movieId);
        if (movie == null) throw new Exception($"Movie with id {movieId} not found");

        movie.Title = updatedMovie.Title;
        movie.Year = updatedMovie.Year;
        movie.DirectorId = updatedMovie.DirectorId;
        movie.Rating = updatedMovie.Rating;

        _context.SaveChanges();
        movie = _context.Movies
            .Include(m => m.Director)
            .FirstOrDefault(m => m.Id == movieId);

        return new MovieResponseVM
        {
            Id = movie!.Id,
            Title = movie.Title,
            Year = movie.Year,
            Rating = movie.Rating,
            Director = new DirectorVM
            {
                Id = movie.Director.Id,
                FullName = movie.Director.FullName
            }
        };
    }

    private Movie? _getMovieById(int movieId)
    {
        return _context.Movies.FirstOrDefault(m => m.Id == movieId);
    }
}