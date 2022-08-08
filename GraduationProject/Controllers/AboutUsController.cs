using GraduationProject.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Controllers
{
    public class AboutUsController : Controller
    {
        private readonly AppDbContext _context;

        public AboutUsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.aboutUs.FirstOrDefaultAsync());
        }
    }
}
