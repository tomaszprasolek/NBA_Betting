using System;
using Microsoft.AspNetCore.Identity;
using NBA_Betting.Data.Enums;

namespace NBA_Betting.Data.Entities
{
    public class LeagueMembership
    {
        public int MembershipId { get; set; }
        public string UserId { get; set; }
        public int LeagueId { get; set; }
        public LeagueRole RoleInLeague { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public IdentityUser User { get; set; }
        public League League { get; set; }
    }
}
