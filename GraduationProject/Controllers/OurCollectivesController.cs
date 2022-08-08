using GraduationProject.DAL;
using GraduationProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GraduationProject.Controllers
{
    public class OurCollectivesController : Controller
    {
        private readonly AppDbContext db;
        public OurCollectivesController(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult> Index()
        {
            OurCollectiveViewModel ocvm = new OurCollectiveViewModel()
            {
                ourCollectives = await db.ourCollectives.ToListAsync(),
            };
            return View(ocvm);
        }

        public async Task<IActionResult> Info(int? id)
        {
            if (id == null) return NotFound();

            var ourCollective = await db.ourCollectives
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ourCollective == null) return NotFound();
            return View(ourCollective);
        }
    }
}
