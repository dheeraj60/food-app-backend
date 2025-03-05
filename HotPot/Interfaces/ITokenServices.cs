using HotPot.Models.DTO;

namespace HotPot.Interfaces
{
    public interface ITokenServices
    {
        public Task<string> GenerateToken(LoginUserDTO user);
    }
}