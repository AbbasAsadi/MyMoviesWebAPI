using Microsoft.EntityFrameworkCore;
using MyMovies.Data.Model;
using MyMovies.Data.Paging;
using MyMovies.Data.ViewModels;
using MyMovies.Exception;

namespace MyMovies.Data.Service;

public class MoviesService(AppDbContext context)
{
    public MovieResponseV2VM AddMovie(MovieVM movie)
    {
        var newMovie = new Movie
        {
            Title = movie.Title,
            Year = movie.Year,
            Rating = movie.Rating,
            DirectorId = movie.DirectorId
        };
        context.Movies.Add(newMovie);
        context.SaveChanges();

        context.Entry(newMovie).Reference(m => m.Director).Load();

        return new MovieResponseV2VM
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

    public List<MovieResponseV1VM> GetAllMovieV1(
        string? sortBy,
        string? sortOrder,
        int? year,
        double? minRating,
        int? pageNumber,
        int? pageSize)
    {
        var allMovie = context
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
            new MovieResponseV1VM
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
            });


        return PaginatedList<MovieResponseV1VM>.Create(result, pageNumber ?? 1, pageSize ?? 5);
    }

    public List<MovieResponseV2VM> GetAllMovieV2(
        string? sortBy,
        string? sortOrder,
        int? year,
        double? minRating,
        int? pageNumber,
        int? pageSize)
    {
        var allMovie = context
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
            new MovieResponseV2VM
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


        return PaginatedList<MovieResponseV2VM>.Create(result, pageNumber ?? 1, pageSize ?? 5);
    }

    public MovieResponseV2VM GetMovieById(int movieId)
    {
        var movie = context.Movies.Include(movie => movie.Director).FirstOrDefault(m => m.Id == movieId);
        return movie == null
            ? throw new NotFoundException($"Movie with id {movieId} not found")
            : new MovieResponseV2VM
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
        context.Movies.Remove(movie);
        context.SaveChanges();
    }


    public MovieResponseV2VM UpdateMovie(int movieId, MovieVM updatedMovie)
    {
        var movie = _getMovieById(movieId);
        if (movie == null) throw new NotFoundException($"Movie with id {movieId} not found");

        movie.Title = updatedMovie.Title;
        movie.Year = updatedMovie.Year;
        movie.DirectorId = updatedMovie.DirectorId;
        movie.Rating = updatedMovie.Rating;

        context.SaveChanges();
        movie = context.Movies
            .Include(m => m.Director)
            .FirstOrDefault(m => m.Id == movieId);

        return new MovieResponseV2VM
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
        return context.Movies.FirstOrDefault(m => m.Id == movieId);
    }
}