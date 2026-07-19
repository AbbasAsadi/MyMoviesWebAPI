using Microsoft.AspNetCore.Mvc;
using MyMovies.Data.Service;

namespace MyMovies.Controller;

public class MovieController(MoviesService moviesService) : ControllerBase
{

}