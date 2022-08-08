using GraduationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.ViewModels
{
    public class ReservationViewModel
    {
        public Concert Concert { get; set; }
        public string SoldTicketIds { get; set; }
    }
}
