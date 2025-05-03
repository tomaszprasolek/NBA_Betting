using NBA_Betting.Data.Entities;

public class Team
{
    public int TeamId { get; set; }
    public string TeamName { get; set; }

    // Navigation properties
    public ICollection<Match> HomeMatches { get; set; }
    public ICollection<Match> AwayMatches { get; set; }
}