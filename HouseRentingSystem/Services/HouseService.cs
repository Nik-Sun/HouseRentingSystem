using HouseRentingSystem.Data;
using HouseRentingSystem.Models;

namespace HouseRentingSystem.Services
{
    public class HouseService : IHouseService
    {
        private ApplicationDbContext context ;
        public HouseService(ApplicationDbContext _context)
        {
            context = _context;
        }
        public IEnumerable<HouseDetailViewModel> GetAllHouses()
        {
            var houses = this.context.Houses
                .Select(h => new HouseDetailViewModel
                {
                    Address = h.Address,
                    Id = h.Id,
                    ImageUrl = h.ImageUrl,
                    Title = h.Title,
                }).ToList();

            return houses;
        }

        public HouseDetailViewModel GetHouseDetail(int id)
        {
            var model = context.Houses.Where(h => h.Id == id)
                  .Select(h => new HouseDetailViewModel
                  {
                      Id = id,
                      Address = h.Address,
                      ImageUrl = h.ImageUrl,
                      Title = h.Title,
                  }).FirstOrDefault();
            return model;
        }
    }
}
