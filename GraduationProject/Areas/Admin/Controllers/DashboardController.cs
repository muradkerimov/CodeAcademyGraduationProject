using GraduationProject.Areas.Admin.Models;
using GraduationProject.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GraduationProject.Models;
using Microsoft.EntityFrameworkCore;


namespace GraduationProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            DashboardViewModels dvm = new DashboardViewModels();
            dvm.ConcertCount = await _context.concerts.CountAsync();
            dvm.BlogCount = await _context.blogs.CountAsync();
            dvm.SpecialMomentsCount = await _context.specialMoments.CountAsync();
            dvm.OurCollectiveCount = await _context.ourCollectives.CountAsync();
            dvm.UserCount = await _context.Users.CountAsync();
            return View(dvm);
        }
    }
}
