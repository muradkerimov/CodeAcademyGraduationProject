using GraduationProject.DAL;
using GraduationProject.Models;
using GraduationProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        public HomeController(AppDbContext _db)
        {
            db = _db;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel hvm = new HomeViewModel()
            {
                sliders = await db.concerts.Take(5).OrderBy(x=>x.Id).Where(x => x.Time > DateTime.Now).ToListAsync(),
                concerts=await db.concerts.OrderBy(x=>x.Time).Where(x=>x.Time>DateTime.Now).Take(3).ToListAsync(), 
                blogs =await db.blogs.Take(3).OrderBy(x=>x.Time).ToListAsync(),
                specialMoments=await db.specialMoments.Include(x=>x.SpecialMomentImages).Take(2).OrderBy(x=>x.Time).ToListAsync(), 
                ourCollectives=await db.ourCollectives.ToListAsync(),
                aboutUs = await db.aboutUs.FirstOrDefaultAsync()
            };
            return View(hvm);
        }

        public IActionResult Basket()
        {
            return View();
        }
    }
}
