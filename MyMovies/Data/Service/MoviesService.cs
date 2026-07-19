using MyMovies.Data.Model;
using MyMovies.Data.ViewModels;

namespace MyMovies.Data.Service;

public class MoviesService(AppDbContext _context)
{
    public Movie AddMovie(MovieVM movie)
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

        return newMovie;


    }

    public List<Movie> GetAllMovie() => _context.Movies.ToList();

    public Movie? GetMovieById(int movieId) => _context.Movies.FirstOrDefault(m => m.Id == movieId);

    public void DeleteMovie(int movieId)
    {
        var movie = GetMovieById(movieId);
        if(movie == null) throw new Exception($"Movie with id {movie} not found");
        _context.Movies.Remove(movie);
    }

    public Movie UpdateMovie(int movieId, MovieVM updatedMovie)
    {
        var movie = GetMovieById(movieId);
        if(movie == null) throw new Exception($"Movie with id {movie} not found");

        movie.Title = updatedMovie.Title;
        movie.Year = updatedMovie.Year;
        movie.DirectorId = updatedMovie.DirectorId;
        movie.Rating = updatedMovie.Rating;

        _context.SaveChanges();
        return movie;

    }
}