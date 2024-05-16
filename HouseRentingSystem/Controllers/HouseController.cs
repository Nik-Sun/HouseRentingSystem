using HouseRentingSystem.Data;
using HouseRentingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HouseRentingSystem.Controllers
{
    public class HouseController : Controller
    {
        private ApplicationDbContext context;

        public HouseController(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IActionResult AllHouses()
        {
            var houses = this.context.Houses
                .Select(h => new HouseDetailViewModel
                {
                    Address = h.Address,
                    Id = h.Id,
                    ImageUrl = h.ImageUrl,
                    Title = h.Title,
                }).ToList();

            return View(houses);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return BadRequest();
            }


            var myHouses = context.Houses
                .Where(h => h.Agent.UserId == currentUserId)
                .Select(h => new HouseDetailViewModel
                {
                    Address = h.Address,
                    Id = h.Id,
                    ImageUrl = h.ImageUrl,
                    Title = h.Title,
                }).ToList();

            return View(myHouses);
        }

        public IActionResult HouseDetails(int id)
        {
            var model = context.Houses.Where(h => h.Id == id)
                .Select(h => new HouseDetailViewModel
                {
                    Id = id,
                     Address = h.Address,
                     ImageUrl= h.ImageUrl,
                     Title= h.Title,
                }).FirstOrDefault();

            if (model == null)
            {
                return BadRequest();
            }
            return View(model);
        }
    }
}
