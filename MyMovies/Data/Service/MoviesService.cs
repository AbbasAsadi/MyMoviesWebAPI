using Microsoft.EntityFrameworkCore;
using MyMovies.Data.Model;
using MyMovies.Data.Paging;
using MyMovies.Data.ViewModels;
using MyMovies.Exception;

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
            DirectorId = movie.DirectorId
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

    public List<MovieResponseVM> GetAllMovie(
        string? sortBy,
        string? sortOrder,
        int? year,
        double? minRating,
        int? pageNumber,
        int? pageSize)
    {
        var allMovie = _context
            .Movies
            .Include(movie => movie.Director)
            .AsQueryable();


        if (year != null) allMovie = allMovie.Where(movie => movie.Year == year);

        if (minRating != null) allMovie = allMovie.Where(movie => movie.Rating > minRating);

        if (sortBy != null && sortOrder != null)
        {
            if (sortOrder == "desc")
                switch (sortBy)
                {
                    case "title":
                        allMovie = allMovie.OrderByDescending(movie => movie.Title);
                        break;
                    case "rating":
                        allMovie = allMovie.OrderByDescending(movie => movie.Rating);
                        break;
                }
            else if (sortOrder == "asc")
                switch (sortBy)
                {
                    case "title":
                        allMovie = allMovie.OrderBy(movie => movie.Title);
                        break;
                    case "rating":
                        allMovie = allMovie.OrderBy(movie => movie.Rating);
                        break;
                }
        }

        var result = allMovie.Select(movie =>
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
            });


        return PaginatedList<MovieResponseVM>.Create(result, pageNumber ?? 1, pageSize ?? 5);
    }

    public MovieResponseVM GetMovieById(int movieId)
    {
        var movie = _context.Movies.Include(movie => movie.Director).FirstOrDefault(m => m.Id == movieId);
        return movie == null
            ? throw new NotFoundException($"Movie with id {movieId} not found")
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
        if (movie == null) throw new NotFoundException($"Movie with id {movieId} not found");
        _context.Movies.Remove(movie);
        _context.SaveChanges();
    }


    public MovieResponseVM UpdateMovie(int movieId, MovieVM updatedMovie)
    {
        var movie = _getMovieById(movieId);
        if (movie == null) throw new NotFoundException($"Movie with id {movieId} not found");

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