using HouseRentingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    public class HouseController : Controller
    {

        private List<HouseDetailViewModel> houses = new List<HouseDetailViewModel>
        {
            new HouseDetailViewModel()
            {
                Id=1,
                Title = "Lake House",
                Address = "Sofia,Bulgaria 33",
                ImageUrl = "https://media1.ispdd.com/projects/big/proekt-za-kashta-Varna-R5rH4-68627028273658452.jpg"
            },
              new HouseDetailViewModel()
            {
                 Id = 2,
                Title = "Another house",
                Address = "Sofia,Drujba 45",
                ImageUrl = "https://photo.barnes-international.com/annonces/bms/181/xl/733272875e7030f2ef7eb6.19188976_7dbf53b483_1920.jpg"
            },
                new HouseDetailViewModel()
            {
                    Id = 3,
                Title = "Other house",
                Address = "Sofia,Lulin 22",
                ImageUrl = "https://www.rocketmortgage.com/resources-cmsassets/RocketMortgage.com/Article_Images/Large_Images/TypesOfHomes/types-of-homes-hero.jpg"
            }
        };
        public IActionResult AllHouses()
        {

            return View(houses);
        }

        //[HttpGet(nameof(HouseDetails) + "/{id}")]
        public IActionResult HouseDetails(int id)
        {
            var model = houses.Where(h => h.Id == id).FirstOrDefault();
            return View(model);
        }
    }
}
