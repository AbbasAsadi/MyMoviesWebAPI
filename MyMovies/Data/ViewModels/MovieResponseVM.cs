namespace MyMovies.Data.ViewModels;

public class MovieResponseVM
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public double Rating { get; set; }
    public DirectorVM Director { get; set; }
}