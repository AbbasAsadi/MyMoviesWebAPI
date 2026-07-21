using Microsoft.AspNetCore.Mvc;
using MyMovies.Data.Service;
using MyMovies.Data.ViewModels;

namespace MyMovies.Controller;

[Route("api/[controller]")]
[ApiController]
public class MovieController(MoviesService moviesService) : ControllerBase
{
    [HttpGet("movies")]
    public IActionResult GetAllMovies(
        string? sortBy,
        string? sortOrder,
        int? year,
        double? minRating,
        int? pageNumber,
        int? pageSize)
    {
        var movies = moviesService.GetAllMovie(
            sortBy,
            sortOrder,
            year,
            minRating,
            pageNumber,
            pageSize);
        return Ok(movies);
    }

    [HttpGet("movies/{id}")]
    public IActionResult GetMovie(int id)
    {
        try
        {
            var movies = moviesService.GetMovieById(id);
            return Ok(movies);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("movies")]
    public IActionResult AddMovie([FromBody] MovieVM movie)
    {
        var result = moviesService.AddMovie(movie);
        return Ok(result);
    }

    [HttpDelete("movies/{id}")]
    public IActionResult DeleteMovie(int id)
    {
        try
        {
            moviesService.DeleteMovie(id);
            return Ok();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("movies/{id}")]
    public IActionResult UpdateMovie(int id, [FromBody] MovieVM movie)
    {
        try
        {
            var result = moviesService.UpdateMovie(id, movie);
            return Ok(result);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}