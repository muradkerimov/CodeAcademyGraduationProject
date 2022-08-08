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
using System.IO;
using GraduationProject.Utils;
using Microsoft.AspNetCore.Authorization;

namespace GraduationProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public BlogsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Admin/Blogs
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.blogs.ToListAsync());
        }

        // GET: Admin/Blogs/Details/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blogs/Create
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Subtitle,ImageFile,Image,Time,Id")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                if (!blog.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "File must be an image.");
                    return View();
                }
                if (!blog.ImageFile.IsValidSize(5000))
                {
                    ModelState.AddModelError("ImageFile", "Max file size is 5000 kbs.");
                    return View();
                }
                blog.Image = await blog.ImageFile.Upload(_env.WebRootPath, @"assets\img\Upload");
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Admin/Blogs/Edit/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,ImageFile,Subtitle,Image,Time,Id")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                if (!blog.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "File must be an image.");
                    return View();
                }
                if (!blog.ImageFile.IsValidSize(5000))
                {
                    ModelState.AddModelError("ImageFile", "Max file size is 5000 kbs.");
                    return View();
                }
                string filePath = Path.Combine(_env.WebRootPath, @"assets\img\Upload", blog.Image);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                blog.Image = await blog.ImageFile.Upload(_env.WebRootPath, @"assets\img\Upload");
                _context.Update(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Admin/Blogs/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Admin/Blogs/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.blogs.FindAsync(id);
            _context.blogs.Remove(blog);
            string filePath = Path.Combine(_env.WebRootPath, @"assets\img\Upload", blog.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.blogs.Any(e => e.Id == id);
        }
    }
}
