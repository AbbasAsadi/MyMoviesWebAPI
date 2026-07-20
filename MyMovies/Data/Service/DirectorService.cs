using Microsoft.EntityFrameworkCore;
using MyMovies.Data.ViewModels;

namespace MyMovies.Data.Service;

public class DirectorService(AppDbContext _context)
{
    public List<MovieResponseVM> GetAllMoviesByDirectorId(int directorId)
    {
        var director = _context.Directors.Include(director => director.Movies)
            .FirstOrDefault(director => director.Id == directorId);
        return director == null
            ? throw new Exception($"Director with id {directorId} not found")
            : director
                .Movies
                .Select(movie => new MovieResponseVM
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
}