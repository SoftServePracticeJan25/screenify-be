using Domain.DTOs.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAvatarService
    {
        Task<UserInfoDto?> UploadAvatarAsync(IFormFile file, ClaimsPrincipal user);
        Task<string?> GetAvatarUrlAsync(ClaimsPrincipal user);
    }

}
