using ChatApp.data.DataModels.DTOs.Users;
using ChatApp.data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.data.Services.Interface
{
    public interface IUserService
    {
        Task<UserEntity?> GetUserByEmailAsync(LoginDto loginDto);
        Task<UserEntity?> GetUserByEmailAsync(string userEmail);
    }
}
