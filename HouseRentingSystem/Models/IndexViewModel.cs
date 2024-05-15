namespace HouseRentingSystem.Models
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            Houses = new List<HouseIndexViewModel>();
        }
        public int TotalRents { get; set; }
        public int TotalHouses { get; set; }

        public List<HouseIndexViewModel> Houses { get; set; }
    }
}
