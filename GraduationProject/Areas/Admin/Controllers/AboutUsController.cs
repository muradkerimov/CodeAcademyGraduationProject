using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GraduationProject.DAL;
using GraduationProject.Models;
using Microsoft.AspNetCore.Hosting;
using GraduationProject.Utils;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace GraduationProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutUsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public AboutUsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Admin/AboutUs
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.aboutUs.ToListAsync());
        }

        // GET: Admin/AboutUs/Details/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutUs = await _context.aboutUs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutUs == null)
            {
                return NotFound();
            }

            return View(aboutUs);
        }

        // GET: Admin/AboutUs/Create
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AboutUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Subtitle,Image,ImageFile,Id")] AboutUs aboutUs)
        {
            if (ModelState.IsValid)
            {
                if (!aboutUs.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "File must be an image.");
                    return View();
                }
                if (!aboutUs.ImageFile.IsValidSize(5000))
                {
                    ModelState.AddModelError("ImageFile", "Max file size is 5000 kbs.");
                    return View();
                }
                aboutUs.Image = await aboutUs.ImageFile.Upload(_env.WebRootPath, @"assets\img\Upload");
                _context.Add(aboutUs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutUs);
        }

        // GET: Admin/AboutUs/Edit/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutUs = await _context.aboutUs.FindAsync(id);
            if (aboutUs == null)
            {
                return NotFound();
            }
            return View(aboutUs);
        }

        // POST: Admin/AboutUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Subtitle,ImageFile,Image,Id")] AboutUs aboutUs)
        {
            if (id != aboutUs.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (!aboutUs.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "File must be an image.");
                    return View();
                }
                if (!aboutUs.ImageFile.IsValidSize(5000))
                {
                    ModelState.AddModelError("ImageFile", "Max file size is 5000 kbs.");
                    return View();
                }
                string filePath = Path.Combine(_env.WebRootPath, @"assets\img\Upload", aboutUs.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                aboutUs.Image = await aboutUs.ImageFile.Upload(_env.WebRootPath, @"assets\img\Upload");
                _context.Update(aboutUs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutUs);
        }

        // GET: Admin/AboutUs/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutUs = await _context.aboutUs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutUs == null)
            {
                return NotFound();
            }

            return View(aboutUs);
        }

        // POST: Admin/AboutUs/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aboutUs = await _context.aboutUs.FindAsync(id);
            string filePath = Path.Combine(_env.WebRootPath, @"assets\img\Upload\Category", aboutUs.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            _context.aboutUs.Remove(aboutUs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutUsExists(int id)
        {
            return _context.aboutUs.Any(e => e.Id == id);
        }
    }
}
