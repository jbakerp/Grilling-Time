using System.Security.Policy;

namespace Capstone.Models
{
    public class MenuItem
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } = 0;
        public string Image { get; set; }
        public bool IsVegan { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsGlutenFree { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsGuestBrought { get; set; }
        public int CookoutId { get; set; }
    }
}
