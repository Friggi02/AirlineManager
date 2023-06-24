using AirlineManager.dal.Entities;

namespace AirLineManager.webapi.Models
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public List<ReservationModel>? Reservations { get; set; }
        public string? Role { get; set; }
    }
}
