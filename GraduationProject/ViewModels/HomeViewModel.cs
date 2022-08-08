using GraduationProject.Models;
using System.Collections.Generic;

namespace GraduationProject.ViewModels
{
    public class HomeViewModel
    {
        public List<Concert> sliders { get; set; }
        public List<Blog> blogs { get; set; }
        public List<Concert> concerts { get; set; }
        public List<OurCollective> ourCollectives { get; set; }
        public List<SpecialMoment> specialMoments { get; set; }
        public AboutUs aboutUs { get; set; }

    }
}
