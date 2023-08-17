using System.Collections.Generic;

namespace Capstone.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CookoutId { get; set; }
        public bool IsCompleted { get; set; }
        public string Email { get; set; }
        public IList<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

    }
}
