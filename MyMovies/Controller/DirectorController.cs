using Microsoft.AspNetCore.Mvc;
using MyMovies.Data.Service;

namespace MyMovies.Controller;

[Route("api/[controller]")]
[ApiController]
public class DirectorController(DirectorService directorService) : ControllerBase
{
    [HttpGet("directors/{directorId}/movies")]
    public IActionResult GetAllMoviesByDirectorId(int directorId)
    {
        try
        {
            var movies = directorService.GetAllMoviesByDirectorId(directorId);
            return Ok(movies);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}