using GraduationProject.DAL;
using GraduationProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            BlogViewModel bvm = new BlogViewModel()
            {
             blogs=await _context.blogs.ToListAsync(),
            };
            return View(bvm);
        }
        public async Task<IActionResult> Info(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _context.blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null) return NotFound();
            return View(blog);

        }
    }
}
