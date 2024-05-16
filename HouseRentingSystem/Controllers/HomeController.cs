using HouseRentingSystem.Data;
using HouseRentingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext context;

        public HomeController(ApplicationDbContext ctx)
        {
            context = ctx;
        }


        public IActionResult Index()
        {
            var model = new IndexViewModel();

            var houses = context.Houses
                .Select( h => new HouseIndexViewModel 
                { 
                    ImageUrl = h.ImageUrl,
                    Title = h.Title,
                    IsRented = h.RenterId != null
                }).ToList();

            model.Houses = houses;
            model.TotalHouses = houses.Count;
            model.TotalRents = houses.Where(h => h.IsRented == true).Count();

            return View(model);
        }

      

       
    }
}
