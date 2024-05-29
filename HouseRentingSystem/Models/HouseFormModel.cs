using System.ComponentModel.DataAnnotations;

namespace HouseRentingSystem.Models
{
    public class HouseFormModel
    {

        public HouseFormModel()
        {
            Categories = new List<HouseCategoryViewModel>();
        }
        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 50)]
        public string Description { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 30)]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Image Url")]
        public string ImageURl { get; set; }

        [Range(0.00,1000.00)]
        [Display(Name = "Price Per Month")]
        public decimal PricePerMonth { get; set; }

        public int CategoryId { get; set; }
        public List<HouseCategoryViewModel> Categories { get; set; }
    }
}

//Title – a string with min length 10 and max length 50 (required)
//Address – a string with min length 30 and max length 150 (required)
//Description – a string with min length 50 and max length 500 (required)
//ImageUrl – a string (required)
//PricePerMonth – a decimal with min value 0 and max value 2000 (required)
//CategoryId – an integer (required)
//Category – a Category object
//AgentId – a Guid (required)
//Agent – an Agent object
//RenterId – a string
//Renter – an IdentityUser object (the default user class in ASP.NET)