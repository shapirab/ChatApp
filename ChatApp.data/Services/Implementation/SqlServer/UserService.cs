using ChatApp.data.DataModels.DTOs.Users;
using ChatApp.data.DataModels.Entities;
using ChatApp.data.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.data.Services.Implementation.SqlServer
{
    public class UserService(SignInManager<UserEntity> signInManager) : IUserService
    {
        public async Task<UserEntity?> GetUserByEmailAsync(LoginDto loginDto)
        {
            UserEntity? user = await signInManager.UserManager.FindByEmailAsync(loginDto.Email);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string userEmail)
        {
            UserEntity? user = await signInManager.UserManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                return user;
            }
            return null;
        }
    }
}
