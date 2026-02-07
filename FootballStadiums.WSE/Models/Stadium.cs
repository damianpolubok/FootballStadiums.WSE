namespace FootballStadiums.WSE.Models;

public class Stadium
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Address Address { get; set; } = new Address();
    public string ImageUrl { get; set; } = string.Empty;
    public ICollection<Club> Clubs { get; set; } = new List<Club>();
}