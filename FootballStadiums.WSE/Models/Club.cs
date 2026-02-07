namespace FootballStadiums.WSE.Models;

public class Club
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StadiumId { get; set; }
    public Stadium? Stadium { get; set; }
}