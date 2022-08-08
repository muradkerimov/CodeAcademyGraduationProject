using GraduationProject.DAL;
using GraduationProject.Models;
using GraduationProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject.Controllers
{
    public class ConcertsController : Controller
    {
        private readonly AppDbContext _db;
        public ConcertsController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //return PartialView("_ConcertPartial", _db.concerts.Where(x => x.Time > DateTime.Now).Skip(skip).Take(3).ToList());
            
            ConcertViewModel cvm=new ConcertViewModel();
            cvm.concerts = _db.concerts.Where(x => x.Time > DateTime.Now).Take(3).ToList();
            return View(cvm);
        }

        public IActionResult LoadMoreIndex(int skip = 0)
        {
            return PartialView("_ConcertPartial", _db.concerts.Where(x => x.Time > DateTime.Now).Skip(skip).Take(3).ToList());
            //return View(_db.concerts.Where(x => x.Time > DateTime.Now).ToList());
        }

        public async Task<IActionResult> Info(int? id)
        {
            if (id == null) return NotFound();

            var concert = await _db.concerts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (concert == null) return NotFound();



            return View(concert);
        }
        public async Task<IActionResult> Reservation(int? id)
        {
            if (id == null) return NotFound();

            var concert = await _db.concerts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (concert == null) return NotFound();

            ReservationViewModel rvm = new ReservationViewModel();
            rvm.Concert = concert;
            rvm.SoldTicketIds = "";
            List<Ticket> soldTickets = _db.soldTickets.Where(x => x.ConcertId == concert.Id).ToList();
            foreach (var item in soldTickets)
            {
                rvm.SoldTicketIds += "/" + item.SeatId + "/";
            }

            return View(rvm);
        }

        public async Task<IActionResult> BuyTickets(string ticketdata)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            List<Ticket> tickets = JsonConvert.DeserializeObject<List<Ticket>>(ticketdata);

            foreach (var item in tickets)
            {
                if (TicketExists(item.TicketIdent))  return RedirectToAction("Basket", "Home");
            }

            AppUser loggedUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

            foreach (var item in tickets)
            {
                item.AppUserId = loggedUser.Id;
            }
            _db.soldTickets.AddRange(tickets);
            _db.SaveChanges();
            return View();
        }

        private bool TicketExists(int id)
        {
            return _db.soldTickets.Any(e => e.TicketIdent == id);
        }
    }
}
