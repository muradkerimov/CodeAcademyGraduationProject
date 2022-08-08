using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GraduationProject.DAL;
using GraduationProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace GraduationProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TicketsController : Controller
    {
        private readonly AppDbContext _context;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Tickets
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.soldTickets.Include(t=>t.User).Include(t => t.Concert);
            return View(await appDbContext.ToListAsync());
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.soldTickets
                .Include(t => t.Concert)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.soldTickets
                .Include(t => t.Concert)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.soldTickets.FindAsync(id);
            _context.soldTickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.soldTickets.Any(e => e.Id == id);
        }
    }
}
