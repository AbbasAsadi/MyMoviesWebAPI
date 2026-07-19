namespace MyMovies.Data.Model;

public class Director
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public List<Movie> Movies { get; set; }

}