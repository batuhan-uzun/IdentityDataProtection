using IdentityDataProtection.Dto;
using IdentityDataProtection.Types;

namespace IdentityDataProtection.Service
{
    public interface IUserService
    {
        Task<ServiceMessage> AddUser(AddUserDto user);
        Task<ServiceMessage<UserInfoDto>> LoginUser(LoginUserDto user);
    }
}
