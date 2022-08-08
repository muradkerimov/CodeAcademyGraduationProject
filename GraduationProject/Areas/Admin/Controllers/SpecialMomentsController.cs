using GraduationProject.DAL;
using GraduationProject.Models;
using GraduationProject.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialMomentsController : Controller
    {
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment env;
        public SpecialMomentsController(AppDbContext _db, IWebHostEnvironment _env)
        {
            db = _db;
            env = _env;
        }
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Index()
        {
            var appDbContext = db.specialMoments.Include(s => s.SpecialMomentImages);
            return View(await appDbContext.ToListAsync());
        }
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return RedirectToAction("Index", "SpecialMoments");

            var specialMoment = await db.specialMoments.FirstOrDefaultAsync(m => m.Id == id);
            if (specialMoment == null)return NotFound();

            return View(specialMoment);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Subtitle,Time,ImageFiles")] SpecialMoment specialMoment)
        {
            if (ModelState.IsValid)
            {
                if (specialMoment.ImageFiles != null && specialMoment.ImageFiles.Length > 0)
                {
                    db.Add(specialMoment);
                    await db.SaveChangesAsync();
                    foreach (IFormFile item in specialMoment.ImageFiles)
                    {
                        if (!item.IsImage())
                        {
                            ModelState.AddModelError("ImageFiles", item.FileName + "is not an image.");
                            db.specialMoments.Remove(db.specialMoments.Find(specialMoment.Id));
                            await db.SaveChangesAsync();
                            return View(specialMoment);
                        }

                        if (!item.IsValidSize(5000))
                        {
                            ModelState.AddModelError("ImageFiles", item.FileName + "is too big.");
                            db.specialMoments.Remove(db.specialMoments.Find(specialMoment.Id));
                            await db.SaveChangesAsync();
                            return View(specialMoment);
                        }

                        SpecialMomentImage smi = new SpecialMomentImage();
                        smi.Image = await item.Upload(env.WebRootPath, @"assets\img\Upload");
                        smi.SpecialMomentId = specialMoment.Id;

                        await db.specialMomentImages.AddAsync(smi);
                    }
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                ModelState.AddModelError("ImageFiles", "At least one image is required");
                return View(specialMoment);
            }
            return View(specialMoment);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["SpecialMomentsImages"] = await db.specialMomentImages.Where(x => x.SpecialMomentId == id).ToListAsync();

            var specialMoment = await db.specialMoments.FirstOrDefaultAsync(x => x.Id == id);
            if (specialMoment == null)
            {
                return NotFound();
            }
            return View(specialMoment);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Subtitle,Time,Id,ImageFiles")] SpecialMoment specialMoment)
        {
            ViewData["SpecialmomentsImages"] = await db.specialMomentImages.Where(x => x.SpecialMomentId == specialMoment.Id).ToListAsync();
            if (id != specialMoment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(specialMoment);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialMomentExists(specialMoment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (specialMoment.ImageFiles != null && specialMoment.ImageFiles.Length > 0)
                {
                    if (specialMoment.ImageFiles.ImagesAreValid())
                    {

                        List<SpecialMomentImage> images = await db.specialMomentImages.Where(x => x.SpecialMomentId == specialMoment.Id).ToListAsync();
                        foreach (SpecialMomentImage item in images)
                        {
                            string filePath = Path.Combine(env.WebRootPath, @"assets\img\Upload", item.Image);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                            db.specialMomentImages.Remove(item);
                        }

                        foreach (IFormFile item in specialMoment.ImageFiles)
                        {
                            SpecialMomentImage smi = new SpecialMomentImage();
                            smi.Image = await item.Upload(env.WebRootPath, @"assets\img\Upload");
                            smi.SpecialMomentId = specialMoment.Id;
                            await db.specialMomentImages.AddAsync(smi);
                        }
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        return View(specialMoment);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(specialMoment);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.specialMoments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specialMoment = await db.specialMoments.FindAsync(id);
            List<SpecialMomentImage> images = await db.specialMomentImages.Where(x => x.SpecialMomentId == specialMoment.Id).ToListAsync();

            foreach (SpecialMomentImage item in images)
            {
                string filePath = Path.Combine(env.WebRootPath, @"assets\img\Upload", item.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                db.specialMomentImages.Remove(item);
            }
            db.specialMoments.Remove(specialMoment);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool SpecialMomentExists(int id)
        {
            return db.specialMoments.Any(e => e.Id == id);
        }
    }
}