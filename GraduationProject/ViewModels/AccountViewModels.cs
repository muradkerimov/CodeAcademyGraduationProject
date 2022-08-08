using GraduationProject.Models;
using System.Collections.Generic;

namespace GraduationProject.ViewModels
{
    public class AccountViewModels
    {
        public List<AppUser> appUsers { get; set; }
        public List<Ticket> concertHisttory { get; set; }
        public List<Ticket> futureConcert { get; set; }

    }
}
