using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Models
{
    public class Concert: Base
    {
        [Required, MaxLength(150)]
        public string Title { get; set; }
        [Required, MaxLength(500)]
        public string Subtitle { get; set; }
        [Required, MaxLength(150)]
        public string Drijor { get; set; }
        public string Image { get; set; }
        [Required, MaxLength(150)]
        public string Program { get; set; }
        [Required, MaxLength(150)]
        public string BaletmeysterArtist { get; set; }
        [Required, MaxLength(150)]
        public string XormeysterIncesenetXadimi { get; set; }
        [Required, MaxLength(150)]
        public string MusiqiRehberiArtist { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        [Required, MaxLength(150)]
        public string Location { get; set; }
        public DateTime Time { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
