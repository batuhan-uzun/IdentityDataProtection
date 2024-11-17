using IdentityDataProtection.Enums;

namespace IdentityDataProtection.Dto
{
    public class UserInfoDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public UserRole UserRole { get; set; }
    }
}
