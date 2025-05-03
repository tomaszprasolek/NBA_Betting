using Microsoft.AspNetCore.Identity;
using NBA_Betting.Data.Entities;
using NBA_Betting.Data.Enums;

public class Bet
{
    public int BetId { get; set; }
    public string UserId { get; set; }
    public int MatchId { get; set; }
    public BetChoice BetChoice { get; set; }
    public int PointsAwarded { get; set; }
    public DateTime CreatedDate { get; set; }

    // Navigation properties
    public IdentityUser User { get; set; }
    public Match Match { get; set; }
}