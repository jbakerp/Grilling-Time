using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace Capstone.Models
{
    public class Cookout
    {
        public int CookoutID { get; set; }
        public int NumberOfAttendees { get; set; }
        public int HostId { get; set; }
        public int ChefId { get; set; }
        public DateTime DateOfEvent { get; set; }
        public Address Location { get; set; }
        public string CookoutName { get; set; }
        public string Description { get; set; }

    }
}
