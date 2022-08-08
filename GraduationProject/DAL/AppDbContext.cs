using GraduationProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<AboutUs> aboutUs { get; set; }
        public DbSet<OurCollective> ourCollectives { get; set; }
        public DbSet<Blog> blogs { get; set; }
        public DbSet<Concert> concerts { get; set; }
        public DbSet<SpecialMoment> specialMoments { get; set; }
        public DbSet<SpecialMomentImage> specialMomentImages { get; set; }
        public DbSet<Ticket> soldTickets { get; set; }
    }
}
