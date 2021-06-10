using System;

namespace API.Entities
{
    public class UserVisit
    {
        public AppUser VisiterUser { get; set; }
        public int VisiterUserId { get; set; }
        public AppUser VisitedUser { get; set; }
        public int VisitedUserId { get; set; }
        public DateTime LastVisit { get; set; }
    }
}