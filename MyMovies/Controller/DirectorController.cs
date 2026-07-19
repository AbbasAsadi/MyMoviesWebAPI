using Microsoft.AspNetCore.Mvc;
using MyMovies.Data.Service;

namespace MyMovies.Controller;

public class DirectorController(DirectorService directorService) : ControllerBase
{
    [HttpGet("directors/{directorId}/movies")]
    public IActionResult GetAllMoviesByDirectorId(int directorId)
    {
        try
        {
            var movies = directorService.GetAllMoviesByDirectorID(directorId);
            return Ok(movies);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}