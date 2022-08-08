using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Models
{
    public class SpecialMoment:Base
    {
        [Required, MaxLength(150)]
        public string Title { get; set; }
        [Required, MaxLength(150)]
        public string Subtitle { get; set; }
        public DateTime Time { get; set; }
        public List<SpecialMomentImage> SpecialMomentImages { get; set; }

        [NotMapped]
        public IFormFile[] ImageFiles { get; set; }
    }
}
