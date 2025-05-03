using System;
using System.Collections.Generic;
using NBA_Betting.Data.Enums;

namespace NBA_Betting.Data.Entities
{
    public class Match
    {
        public int MatchId { get; set; }
        public int LeagueId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public DateTime MatchDateTime { get; set; }
        public MatchResult? Result { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public League League { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public ICollection<Bet> Bets { get; set; }
    }
}
