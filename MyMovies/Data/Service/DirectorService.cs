using Microsoft.EntityFrameworkCore;
using MyMovies.Data.Model;

namespace MyMovies.Data.Service;

public class DirectorService(AppDbContext _context)
{
    public List<Movie> GetAllMoviesByDirectorID(int directorId)
    {
        var director = _context.Directors.Include(director => director.Movies)
            .FirstOrDefault(director => director.Id == directorId);
        return director == null ? throw new Exception($"Director with id {directorId} not found") : director.Movies;
    }
}