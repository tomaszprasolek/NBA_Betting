using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace NBA_Betting.Data.Entities
{
    public class League
    {
        public int LeagueId { get; set; }
        public string LeagueCode { get; set; }
        public string LeagueName { get; set; }
        public string? Description { get; set; }
        public string OwnerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public IdentityUser Owner { get; set; }
        public ICollection<Match> Matches { get; set; }
        public ICollection<LeagueMembership> Memberships { get; set; }
    }
}
