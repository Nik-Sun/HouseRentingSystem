using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseRentingSystem.Data.Entities
{
    public class Agent
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public HashSet<House> ManagedHouses { get; set; }
    }
}
//Id – a Guid, Primary Key
//PhoneNumber – a string with min length 7 and max length 15 (required)
//UserId – a string (required)
//User – an IdentityUser object
//ManagedHouses – a collection of House objects