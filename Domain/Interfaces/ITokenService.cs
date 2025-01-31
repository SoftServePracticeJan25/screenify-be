using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(AppUser user);
        string CreateRefreshToken();
    }
}
