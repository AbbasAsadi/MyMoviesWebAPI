using Microsoft.EntityFrameworkCore;
using MyMovies.Data.ViewModels;
using MyMovies.Exception;

namespace MyMovies.Data.Service;

public class DirectorService(AppDbContext _context)
{
    public List<MovieResponseV2VM> GetAllMoviesByDirectorId(int directorId)
    {
        var director = _context.Directors.Include(director => director.Movies)
            .FirstOrDefault(director => director.Id == directorId);
        return director == null
            ? throw new NotFoundException($"Director with id {directorId} not found")
            : director
                .Movies
                .Select(movie => new MovieResponseV2VM
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