using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Entities;
using HouseRentingSystem.Infrastructure;
using HouseRentingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    public class AgentController : Controller
    {
        private ApplicationDbContext context;

        public AgentController(ApplicationDbContext _context)
        {
            context = _context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Become()
        {
            var isAgent = context.Agents.Any(a => a.User.Id == User.Id());
            if (isAgent)
            {
                return BadRequest();
            }
            var model = new BecomeAgentFormModel();
            return View(model);
        }



        [Authorize]
        [HttpPost]
        public IActionResult Become(BecomeAgentFormModel model)
        {
           

            var isDuplicate = context.Agents.Any(a => a.PhoneNumber == model.PhoneNumber);
            var isRenter = context.Houses.Any(h => h.RenterId == User.Id());  

            if (isDuplicate)
            {
                ModelState.AddModelError(nameof(model.PhoneNumber), "Phone number already exists,try another one.");
            }
            if (isRenter)
            {
                ModelState.AddModelError("House", "Cannot become agent if you have active rents.");
            }
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var agent = new Agent()
            {
                UserId = User.Id()!,
                PhoneNumber = model.PhoneNumber,
            };

            context.Agents.Add(agent);
            context.SaveChanges();
            return RedirectToAction(nameof(HouseController.AllHouses), "House");
        }
    }
}
