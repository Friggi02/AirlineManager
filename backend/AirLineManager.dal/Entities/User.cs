using AirlineManager.dal.Entities;
using Microsoft.AspNetCore.Identity;

namespace AirLineManager.dal.Entities
{
    public class User : IdentityUser
    {
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public bool IsDeleted { get; set; }
    }
}
