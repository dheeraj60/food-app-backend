using HotPot.Models.DTO;

namespace HotPot.Interfaces
{
    public interface IAdminServices
    {
        public Task<LoginUserDTO> LoginAdmin(LoginUserDTO loginUser);
        public Task<LoginUserDTO> RegisterAdmin(LoginUserDTO registerUser);
    }
}