using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MyMovies.Data.Service;
using MyMovies.Data.ViewModels;

namespace MyMovies.Controller.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
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
        var movies = moviesService.GetAllMovieV2(
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
        var movies = moviesService.GetMovieById(id);
        return Ok(movies);
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
        moviesService.DeleteMovie(id);
        return Ok();
    }

    [HttpPut("movies/{id}")]
    public IActionResult UpdateMovie(int id, [FromBody] MovieVM movie)
    {
        var result = moviesService.UpdateMovie(id, movie);
        return Ok(result);
    }
}