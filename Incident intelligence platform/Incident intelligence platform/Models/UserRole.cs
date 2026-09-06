using Microsoft.EntityFrameworkCore;

namespace Incident_intelligence_platform.Models
{
    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public class UserRole
    {
        public int UserId { get; set; }
        public UserModel User { get; set; } = null!;

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

    }
}
