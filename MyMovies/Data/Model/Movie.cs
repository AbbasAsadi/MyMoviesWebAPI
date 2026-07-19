namespace MyMovies.Data.Model;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public double Rating { get; set; }
    public int DirectorId { get; set; }
    public Director Director { get; set; }
}