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
    public class ConcertsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public ConcertsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Admin/Concerts
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.concerts.ToListAsync());
        }

        // GET: Admin/Concerts/Details/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concert = await _context.concerts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (concert == null)
            {
                return NotFound();
            }

            return View(concert);
        }

        // GET: Admin/Concerts/Create
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Concerts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Subtitle,ImageFile,Drijor,Image,Program,BaletmeysterArtist,XormeysterIncesenetXadimi,MusiqiRehberiArtist,MinPrice,MaxPrice,Location,Time,Id")] Concert concert)
        {
            if (ModelState.IsValid)
            {
                if (!concert.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "File must be an image.");
                    return View();
                }
                if (!concert.ImageFile.IsValidSize(5000))
                {
                    ModelState.AddModelError("ImageFile", "Max file size is 5000 kbs.");
                    return View();
                }
                concert.Image = await concert.ImageFile.Upload(_env.WebRootPath, @"assets\img\Upload");
                _context.Add(concert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concert);
        }

        // GET: Admin/Concerts/Edit/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concert = await _context.concerts.FindAsync(id);
            if (concert == null)
            {
                return NotFound();
            }
            return View(concert);
        }

        // POST: Admin/Concerts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,ImageFile,Subtitle,Drijor,Image,Program,BaletmeysterArtist,XormeysterIncesenetXadimi,MusiqiRehberiArtist,MinPrice,MaxPrice,Location,Time,Id")] Concert concert)
        {
            if (id != concert.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!concert.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "File must be an image.");
                    return View();
                }
                if (!concert.ImageFile.IsValidSize(5000))
                {
                    ModelState.AddModelError("ImageFile", "Max file size is 5000 kbs.");
                    return View();
                }
                string filePath = Path.Combine(_env.WebRootPath, @"assets\img\Upload", concert.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                concert.Image = await concert.ImageFile.Upload(_env.WebRootPath, @"assets\img\Upload");
                _context.Update(concert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concert);
        }

        // GET: Admin/Concerts/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concert = await _context.concerts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (concert == null)
            {
                return NotFound();
            }

            return View(concert);
        }

        // POST: Admin/Concerts/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concert = await _context.concerts.FindAsync(id);
            string filePath = Path.Combine(_env.WebRootPath, @"assets\img\Upload", concert.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            _context.concerts.Remove(concert);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConcertExists(int id)
        {
            return _context.concerts.Any(e => e.Id == id);
        }
    }
}
