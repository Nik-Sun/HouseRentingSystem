using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseRentingSystem.Data.Entities
{
    public class House
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(150)]
        public string Address { get; set; }


        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public decimal PricePerMonth { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        [ForeignKey(nameof(Agent))]
        public Guid AgentId { get; set; }
        public Agent Agent { get; set; }

        [ForeignKey(nameof(Renter))]
        public string? RenterId { get; set; }

        public IdentityUser? Renter { get; set; }
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