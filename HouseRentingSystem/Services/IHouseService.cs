using HouseRentingSystem.Models;

namespace HouseRentingSystem.Services
{
    public interface IHouseService
    {
        public IEnumerable<HouseDetailViewModel> GetAllHouses();

        public HouseDetailViewModel GetHouseDetail(int id);
    }
}
