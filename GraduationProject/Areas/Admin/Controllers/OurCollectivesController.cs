using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GraduationProject.DAL;
using GraduationProject.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using GraduationProject.Utils;
using Microsoft.AspNetCore.Authorization;

namespace GraduationProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OurCollectivesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public OurCollectivesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Admin/OurCollectives
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ourCollectives.ToListAsync());
        }

        // GET: Admin/OurCollectives/Details/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ourCollective = await _context.ourCollectives
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ourCollective == null)
            {
                return NotFound();
            }

            return View(ourCollective);
        }

        // GET: Admin/OurCollectives/Create
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/OurCollectives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([Bind("Title,ImageFile,Subtitle,Image,Id")] OurCollective ourCollective)
        {
            if (ModelState.IsValid)
            {
                if (!ourCollective.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "File must be an image.");
                    return View();
                }
                if (!ourCollective.ImageFile.IsValidSize(5000))
                {
                    ModelState.AddModelError("ImageFile", "Max file size is 5000 kbs.");
                    return View();
                }
                ourCollective.Image = await ourCollective.ImageFile.Upload(_env.WebRootPath, @"assets\img\Upload");
                _context.Add(ourCollective);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ourCollective);
        }

        // GET: Admin/OurCollectives/Edit/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ourCollective = await _context.ourCollectives.FindAsync(id);
            if (ourCollective == null)
            {
                return NotFound();
            }
            return View(ourCollective);
        }

        // POST: Admin/OurCollectives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,ImageFile,Subtitle,Image,Id")] OurCollective ourCollective)
        {
            if (id != ourCollective.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!ourCollective.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "File must be an image.");
                    return View();
                }
                if (!ourCollective.ImageFile.IsValidSize(5000))
                {
                    ModelState.AddModelError("ImageFile", "Max file size is 5000 kbs.");
                    return View();
                }
                string filePath = Path.Combine(_env.WebRootPath, @"assets\img\Upload", ourCollective.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                ourCollective.Image = await ourCollective.ImageFile.Upload(_env.WebRootPath, @"assets\img\Upload");
                _context.Update(ourCollective);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ourCollective);
        }

        // GET: Admin/OurCollectives/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ourCollective = await _context.ourCollectives
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ourCollective == null)
            {
                return NotFound();
            }

            return View(ourCollective);
        }

        // POST: Admin/OurCollectives/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ourCollective = await _context.ourCollectives.FindAsync(id);
            string filePath = Path.Combine(_env.WebRootPath, @"assets\img\Upload", ourCollective.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            _context.ourCollectives.Remove(ourCollective);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OurCollectiveExists(int id)
        {
            return _context.ourCollectives.Any(e => e.Id == id);
        }
    }
}
