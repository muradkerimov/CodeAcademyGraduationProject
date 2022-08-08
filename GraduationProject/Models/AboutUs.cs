using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Models
{
    public class AboutUs: Base
    {
        [Required, MaxLength(150)]
        public string Title { get; set; }
        [Required, MaxLength(500)]
        public string Subtitle { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
