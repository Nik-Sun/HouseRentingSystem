using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Entities;
using HouseRentingSystem.Infrastructure;
using HouseRentingSystem.Models;
using HouseRentingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HouseRentingSystem.Controllers
{
    public class HouseController : Controller
    {
        private ApplicationDbContext context;
        private IHouseService service;

        public HouseController(ApplicationDbContext _context,IHouseService service)
        {
            this.service = service;
            context = _context;
        }
        public IActionResult AllHouses()
        {
            

            var houses = service.GetAllHouses(); 


            return View(houses);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var currentUserId = this.User.Id();
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
            var model = service.GetHouseDetail(id);

            if (model == null)
            {
                return BadRequest();
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            var userId = User.Id();
            var isAgent = context.Agents.Any(a => a.UserId == userId);
            if (isAgent == false)
            {
                return RedirectToAction(nameof(AgentController.Become), "Agents");
            }

            var model = new HouseFormModel();

            model.Categories = GetCategories(); ;

            return View(model);
        }

        public IActionResult Add(HouseFormModel model)
        {
            var userId = User.Id();
           

            if (ModelState.IsValid == false)
            {
                model.Categories = GetCategories();
                return View(model);
            }
            var agentId = context.Agents.Where(a => a.UserId == userId).FirstOrDefault().Id;
            var entity = new House
            {
                Address = model.Address,
                PricePerMonth = model.PricePerMonth,
                Title = model.Title,
                ImageUrl = model.ImageURl,
                CategoryId = model.CategoryId,
                AgentId = agentId,
                Description = model.Description,
            };

            context.Houses.Add(entity);
            context.SaveChanges();
            return RedirectToAction(nameof(HouseDetails),new {Id = entity.Id});
        }
        private List<HouseCategoryViewModel> GetCategories()
        {
            return context.Categories
                .Select(c => new HouseCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();
        }
        [Authorize]
        public IActionResult Edit(int id)
        {
            var house = this.context.Houses.Find(id);

            if (house is null)
            {
                return BadRequest();
            }

            var agent = this.context.Agents.FirstOrDefault(a => a.Id == house.AgentId);

            if (agent?.UserId != this.User.Id())
            {
                return Unauthorized();
            }

            var houseModel = new HouseFormModel()
            {
                Title = house.Title,
                Address = house.Address,
                Description = house.Description,
                ImageURl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                CategoryId = house.CategoryId,
                Categories = this.GetHouseCategories().ToList(),
            };

            return View(houseModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, HouseFormModel model)
        {
            var house = this.context.Houses.Find(id);
            if (house is null)
            {
                return this.View();
            }

            var agent = this.context.Agents.FirstOrDefault(a => a.Id == house.AgentId);

            if (agent?.UserId != this.User.Id())
            {
                return Unauthorized();
            }

            if (!this.context.Categories.Any(c => c.Id == model.CategoryId))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = this.GetHouseCategories().ToList();

                return View(model);
            }

            house.Title = model.Title;
            house.Address = model.Address;
            house.Description = model.Description;
            house.ImageUrl = model.ImageURl;
            house.PricePerMonth = model.PricePerMonth;
            house.CategoryId = model.CategoryId;

            this.context.SaveChanges();

            return RedirectToAction(nameof(HouseDetails), new { id = house.Id });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var house = this.context.Houses.Find(id);

            if (house is null)
            {
                return BadRequest();
            }

            var agent = this.context.Agents.FirstOrDefault(a => a.Id == house.AgentId);

            if (agent?.UserId != this.User.Id())
            {
                return Unauthorized();
            }

            var model = new HouseViewModel()
            {
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl
            };

            return View(model);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Delete(HouseViewModel model)
        {
            var house = this.context.Houses.Find(model.Id);

            if (house is null)
            {
                return BadRequest();
            }

            var agent = this.context.Agents.FirstOrDefault(a => a.Id == house.AgentId);

            if (agent?.UserId != this.User.Id())
            {
                return Unauthorized();
            }

            this.context.Houses.Remove(house);
            this.context.SaveChanges();

            return RedirectToAction(nameof(AllHouses));
        }


        //[HttpPost]
        [Authorize]
        public IActionResult Rent(int id)
        {
            var house = this.context.Houses.Find(id);
            if (house is null || house.RenterId is not null)
            {
                return BadRequest();
            }

            if (this.context.Agents.Any(a => a.UserId == this.User.Id()))
            {
                return Unauthorized();
            }

            house.RenterId = this.User.Id();
            this.context.SaveChanges();

            return RedirectToAction(nameof(Mine));
        }

        //[HttpPost]
        [Authorize]
        public IActionResult Leave(int id)
        {
            var house = this.context.Houses.Find(id);
            if (house is null || house.RenterId is null)
            {
                return BadRequest();
            }

            if (house.RenterId != this.User.Id())
            {
                return Unauthorized();
            }

            house.RenterId = null;
            this.context.SaveChanges();

            return RedirectToAction(nameof(Mine));
        }

        private IEnumerable<HouseCategoryViewModel> GetHouseCategories()
            => this.context
                .Categories
                .Select(c => new HouseCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
    }
}
