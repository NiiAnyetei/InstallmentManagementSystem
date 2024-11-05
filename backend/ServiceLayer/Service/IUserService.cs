using DataLayer.Models.DTOs;
using DataLayer.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public interface IUserService
    {
        Task<LoginUserResponseDto> LoginAsync(LoginUserDto user);
        Task<NewRefreshTokenResponseDto> RefreshTokenAsync(NewRefreshTokenDto newRefreshTokenDto);
        Task<UserDto> CreateAsync(NewUserDto user);
        Task<UserDto> GetAsync(string username);
        Task<User> GetUserByUsernameAsync(string username);
        Task<UserDto> UpdateAsync(string username, UpdatedUserDto updatedUser);
    }
}
