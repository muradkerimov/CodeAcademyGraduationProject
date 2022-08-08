using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace GraduationProject.Areas.Admin.Models
{
    public class DashboardViewModels
    {
        public int BlogCount { get; set; }
        public int ConcertCount { get; set; }
        public int ConcertVideoCount { get; set; }
        public int SpecialMomentsCount { get; set; }
        public int OurCollectiveCount { get; set; }
        public int UserCount { get; set; }
    }
}
