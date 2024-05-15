using System.ComponentModel.DataAnnotations;

namespace HouseRentingSystem.Data.Entities
{
    public class Category
    {

        public Category()
        {
            Houses = new HashSet<House>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public HashSet<House> Houses { get; set; }
    }
}
