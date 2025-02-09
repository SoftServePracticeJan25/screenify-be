using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(AppUser user, List<string> roles); 
        string CreateRefreshToken();
    }
}

