using System.ComponentModel.DataAnnotations;

namespace HouseRentingSystem.Models
{
    public class BecomeAgentFormModel
    {
        [Required]
        [StringLength(15, MinimumLength = 7)]
        [Display(Name = "Phone number")]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
    }
}
