﻿using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Authorization;
using HouseRentingSystem.Data;
using HouseRentingSystem.Infrastructure;
using HouseRentingSystem.Models.Houses;
using HouseRentingSystem.Services.Houses;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Houses.Models;

namespace HouseRentingSystem.Controllers
{
    public class HousesController : Controller
    {
        private readonly IHouseService houses;
        private readonly IAgentService agents;

        public HousesController(IHouseService houses, IAgentService agents)
        {
            this.houses = houses;
            this.agents = agents;
        }

        public IActionResult All([FromQuery] AllHousesQueryModel query)
        {
            var queryResult = this.houses.All(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllHousesQueryModel.HousesPerPage);

            query.TotalHousesCount = queryResult.TotalHousesCount;
            query.Houses = queryResult.Houses;

            var houseCategories = this.houses.AllCategoriesNames();
            query.Categories = houseCategories;

            return View(query);
        }

        [Authorize]
        public IActionResult Mine()
        {
            IEnumerable<HouseServiceModel> myHouses;

            var userId = this.User.Id()!;

            if (this.agents.ExistsById(userId))
            {
                var currentAgentId = this.agents.GetAgentId(userId);

                myHouses = this.houses.AllHousesByAgentId(currentAgentId);
            }
            else
            {
                myHouses = this.houses.AllHousesByUserId(userId);
            }

            return View(myHouses);
        }

        public IActionResult Details(int id, string information)
        {
            if (!this.houses.Exists(id))
            {
                return BadRequest();
            }

            var houseModel = this.houses.HouseDetailsById(id);

            if(information != houseModel.GetInformation())
            {
                return BadRequest();
            }

            return View(houseModel);
        }


        [Authorize]
        public IActionResult Add()
        {
            if (!this.agents.ExistsById(this.User.Id()!))
            {
                return RedirectToAction(nameof(AgentsController.Become), "Agents");
            }

            return View(new HouseFormModel
            {
                Categories = this.houses.AllCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(HouseFormModel model)
        {
            if (!this.agents.ExistsById(this.User.Id()!))
            {
                return RedirectToAction(nameof(AgentsController.Become), "Agents");
            }

            if (!this.houses.CategoryExists(model.CategoryId))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId),
                    "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = this.houses.AllCategories();

                return View(model);
            }

            var agentId = this.agents.GetAgentId(this.User.Id()!);

            var newHouseId = this.houses.Create(model.Title, model.Address,
                model.Description, model.ImageUrl, model.PricePerMonth,
                model.CategoryId, agentId);

            return RedirectToAction(nameof(Details),
                new { id = newHouseId, information = model.GetInformation() });
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!this.houses.Exists(id))
            {
                return BadRequest();
            }

            if (!this.houses.HasAgentWithId(id, this.User.Id()!))
            {
                return Unauthorized();
            }

            var house = this.houses.HouseDetailsById(id);

            var houseCategoryId = this.houses.GetHouseCategoryId(house.Id);

            var houseModel = new HouseFormModel()
            {
                Title = house.Title,
                Address = house.Address,
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                CategoryId = houseCategoryId,
                Categories = this.houses.AllCategories()
            };

            return View(houseModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, HouseFormModel model)
        {
            if (!this.houses.Exists(id))
            {
                return this.View();
            }

            if (!this.houses.HasAgentWithId(id, this.User.Id()!))
            {
                return Unauthorized();
            }

            if (!this.houses.CategoryExists(model.CategoryId))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId),
                    "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = this.houses.AllCategories();

                return View(model);
            }

            this.houses.Edit(id, model.Title, model.Address, model.Description,
                model.ImageUrl, model.PricePerMonth, model.CategoryId);

            return RedirectToAction(nameof(Details), 
                new { id = id, information = model.GetInformation() });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!this.houses.Exists(id))
            {
                return BadRequest();
            }

            if (!this.houses.HasAgentWithId(id, this.User.Id()!))
            {
                return Unauthorized();
            }

            var house = this.houses.HouseDetailsById(id);

            var model = new HouseDetailsViewModel()
            {
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(HouseDetailsViewModel model)
        {
            if (!this.houses.Exists(model.Id))
            {
                return BadRequest();
            }

            if (!this.houses.HasAgentWithId(model.Id, this.User.Id()!))
            {
                return Unauthorized();
            }

            this.houses.Delete(model.Id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Rent(int id)
        {
            if (!this.houses.Exists(id))
            {
                return BadRequest();
            }

            if (this.agents.ExistsById(this.User.Id()!))
            {
                return Unauthorized();
            }

            if (this.houses.IsRented(id))
            {
                return BadRequest();
            }

            this.houses.Rent(id, this.User.Id()!);

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Leave(int id)
        {
            if (!this.houses.Exists(id) ||
                !this.houses.IsRented(id))
            {
                return BadRequest();
            }

            if (!this.houses.IsRentedByUserWithId(id, this.User.Id()!))
            {
                return Unauthorized();
            }

            this.houses.Leave(id);

            return RedirectToAction(nameof(Mine));
        }
    }
}
