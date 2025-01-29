﻿using Domain.DTOs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionDto>> GetAllAsync();
        Task<SessionDto> GetByIdAsync(int id);
        Task<SessionDto> CreateAsync(SessionDto session);
        Task<SessionDto> UpdateAsync(int id, SessionDto session);
        Task<bool> DeleteAsync(int id);
    }
}
