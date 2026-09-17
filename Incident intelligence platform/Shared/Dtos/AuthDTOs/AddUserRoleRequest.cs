namespace Shared.Dtos.AuthDTOs
{
    public class AddUserRoleRequest
    {
        public int UserId { get; set; }
        public string RoleName { get; set; } = string.Empty;

    }
}
