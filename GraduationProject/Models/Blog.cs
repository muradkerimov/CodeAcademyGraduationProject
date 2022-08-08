using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Models
{
    public class Blog:Base
    {
        [Required, MaxLength(150)]
        public string Title { get; set; }
        [Required, MaxLength(550)]
        public string Subtitle { get; set; }
        public string Image { get; set; }
        public DateTime Time { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
