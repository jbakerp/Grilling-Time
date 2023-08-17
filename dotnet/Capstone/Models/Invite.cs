namespace Capstone.Models
{
    public class Invite
    {
        public int InviteId { get; set; }
        public string InviteEmail { get; set; }
        public string? PersonName { get; set; } 
        public bool IsSent { get; set; } 
        public int CookoutId { get; set; } 
        public int ResponseStatus { get; set; } = 3;
    }
}
