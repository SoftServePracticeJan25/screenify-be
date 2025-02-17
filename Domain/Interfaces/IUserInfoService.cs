using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs.Account;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserInfoService
    {
        Task<UserInfoDto> GetUserInfo(AppUser user);
        Task<IEnumerable<UserInfoDto>> GetAllUsersAsync();
    }
}