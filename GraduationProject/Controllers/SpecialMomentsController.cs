using GraduationProject.DAL;
using GraduationProject.ViewModels;
using GraduationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Controllers
{
    public class SpecialMomentsController : Controller
    {
        private readonly AppDbContext _db;
        public SpecialMomentsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            SpecialMomentsViewModel smvm = new SpecialMomentsViewModel()
            {
                specialMoments = await _db.specialMoments.Include(x => x.SpecialMomentImages).ToListAsync(),
            };
            return View(smvm);
        }

        public IActionResult Info(int? id)
        {
            if (id == null) return NotFound();
            return View(_db.specialMoments.Include(x => x.SpecialMomentImages).First(m => m.Id == id));
        }
    }
}