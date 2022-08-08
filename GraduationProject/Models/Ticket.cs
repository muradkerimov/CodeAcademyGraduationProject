using System;

namespace GraduationProject.Models
{
    public class Ticket:Base
    {
        public int ConcertId { get; set; }
        public Concert Concert { get; set; }
        public int SeatId { get; set; }
        public string AppUserId { get; set; }
        public AppUser User { get; set; }
        public int TicketIdent { get; set; }
    }
}
