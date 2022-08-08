namespace GraduationProject.Models
{
    public class SpecialMomentImage: Base
    {
        public string Image { get; set; }
        public int SpecialMomentId { get; set; }
        public SpecialMoment SpecialMoment { get; set; }
    }
}
